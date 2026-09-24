import logger from '@/utils/Logger';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { useNavigate } from 'react-router-dom';
import { useBattleNetDataCodeExchangeMutation, useLazyBattleNetDataStateValidateQuery } from '../api/BattleNetData.api';

const BattleNetAuthorizationCallback: React.FC = () => {
    const unauthorizedTimeoutLimit = 4000;

    const { t } = useTranslation('wowGameData');

    const queryParams = new URLSearchParams(location.search);
    const navigate = useNavigate();

    const [stateIsValid, setStateIsValid] = useState(true);

    const [exchangeAuthorizationCode] = useBattleNetDataCodeExchangeMutation();
    const [stateValidate] = useLazyBattleNetDataStateValidateQuery();

    useEffect(() => {
        const state = queryParams.get("state") ?? "";
        const code = queryParams.get("code") ?? "";

        validateStateAsync(state, code);
    }, []);

    useEffect(() => {
        let timeout: NodeJS.Timeout;
        if (!stateIsValid) {
            timeout = setTimeout(() => {
                navigate("/wow-game-data");
            }, unauthorizedTimeoutLimit);
        }

        return () => {
            clearTimeout(timeout);
        }
    }, [stateIsValid]);

    const validateStateAsync = async (state: string, code: string) => {
        try {
            await stateValidate(state).unwrap();
            await getTokenAsync(code);
        } catch (e) {
            logger.error("Failed to validate the battle net authorzation state", e);

            setStateIsValid(false);
        }
    }

    const getTokenAsync = async (code: string) => {
        try {
            await exchangeAuthorizationCode({ authorizationCode: code }).unwrap();
            navigate("/wow-game-data");
        } catch (error) {
            console.error("Failed auth to battle net game api data:", error);
        }
    }

    return (
        <div className="authorization-callback">
            {stateIsValid
                ? <div className="successful">{t("ObtainingWoWData")}</div>
                : <div className="failed">{t("FailedObtainWoWData")}</div>
            }
        </div>
    );
}

export default BattleNetAuthorizationCallback;