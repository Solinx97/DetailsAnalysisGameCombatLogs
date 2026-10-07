import { fileURLToPath, URL } from 'node:url';

import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import { env } from 'process';
import { defineConfig, loadEnv } from 'vite';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
}

const logsEndpoints = (target: string, changeOrigin: boolean, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/Logs`]: { target, changeOrigin, secure: false },
    }
}

const battleNetDataEnpoints = (target: string, changeOrigin: boolean, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/BattleNetIdentity`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/WoWCharacter`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/WoWAccount`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/WoWData`]: { target, changeOrigin, secure: false },
    }
}

const gameLogsEnpoints = (target: string, changeOrigin: boolean, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/Boss`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CombatAbility`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CombatLog`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/Combat`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/Dashboard`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/BossMap`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/Unit`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/UnitCast`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/UnitHealth`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/UnitPosition`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/UnitAura`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/UnitPreAura`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CombatPlayer`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CombatPlayerDeath`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/DamageDone`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/DamageDoneGeneral`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/DamageTaken`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/DamageTakenGeneral`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/HealDone`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/HealDoneGeneral`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/ResourceRecovery`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/ResourceRecoveryGeneral`]: { target, changeOrigin, secure: false },
    }
}

const communityEndpoints = (target: string, changeOrigin: boolean, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/Community`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CommunityDiscussion`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CommunityDiscussionComment`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CommunityUser`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/InviteToCommunity`]: { target, changeOrigin, secure: false },
    }
}

const userEndpoints = (target: string, changeOrigin: boolean, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/User`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/Authentication`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/Customer`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/Friend`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/RequestToConnect`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/Identity`]: { target, changeOrigin, secure: false },
    }
}

const feedEndpoints = (target: string, changeOrigin: boolean, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/UserFeed`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/UserPost`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/UserPostLike`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/UserPostDislike`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/UserPostComment`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CommunityPost`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CommunityPostLike`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CommunityPostDislike`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/CommunityPostComment`]: { target, changeOrigin, secure: false },
    }
}

const chatEndpoints = (target: string,  changeOrigin: boolean, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/PersonalChat`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/PersonalChatMessage`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/GroupChat`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/GroupChatMessage`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/GroupChatUser`]: { target, changeOrigin, secure: false },
        [`^/api/${apiVersion}/VoiceChat`]: { target, changeOrigin, secure: false },
    }
}

const notificationEnddpoints = (target: string, changeOrigin: boolean, apiVersion: string) => {
    return {
        [`^/api/${apiVersion}/Notification`]: { target, changeOrigin, secure: false },
    }
}

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    const apiVersion = env.VITE_API_VERSION ? env.VITE_API_VERSION : 'v1';
    const target = env.VITE_APP_SERVER_PROXY_URL || 'http://localhost:5000';
    const changeOrigin = env.VITE_CHANGE_ORIGIN === "true" ? true : false;

    return {
        plugins: [plugin()],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url))
            }
        },
        server: {
            proxy: {
                ...logsEndpoints(target, changeOrigin, apiVersion),
                ...battleNetDataEnpoints(target, changeOrigin, apiVersion),
                ...gameLogsEnpoints(target, changeOrigin, apiVersion),
                ...communityEndpoints(target, changeOrigin, apiVersion),
                ...userEndpoints(target, changeOrigin, apiVersion),
                ...feedEndpoints(target, changeOrigin, apiVersion),
                ...chatEndpoints(target, changeOrigin, apiVersion),
                ...notificationEnddpoints(target, changeOrigin, apiVersion),
            },
            port: parseInt(env.VITE_DEV_SERVER_PORT || '5173', 10)
        }
    }
})
