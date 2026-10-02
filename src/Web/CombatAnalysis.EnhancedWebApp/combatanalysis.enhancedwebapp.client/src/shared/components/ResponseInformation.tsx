import type { SerializedError } from '@reduxjs/toolkit';
import type { FetchBaseQueryError } from '@reduxjs/toolkit/query';
import type React from 'react';
import { useEffect, useState } from 'react';

interface ResponseErorrInformationProps {
    error: FetchBaseQueryError | SerializedError | undefined;
    watchParams?: string[];
    watchNumberParams?: number[];
    isLoading?: boolean;
}

const ResponseInformation: React.FC<ResponseErorrInformationProps> = ({ error, watchParams, watchNumberParams, isLoading }) => {
    const [isSkipRequest, setIsSkipRequest] = useState<boolean>(false);

    useEffect(() => {
        if (watchParams) {
            setIsSkipRequest(watchParams.filter(x => x.trim().length > 0).length < watchParams.length);
        }
    }, watchParams);

    useEffect(() => {
        if (watchNumberParams) {
            setIsSkipRequest(watchNumberParams.filter(x => x > 0).length < watchNumberParams.length);
        }
    }, watchNumberParams);

    if (isSkipRequest) {
        return (<div>No data</div>);
    }

    if (error && 'status' in error && error.status === 500) {
        return (<div>Ups, something wrong</div>);
    }

    if (error && 'status' in error && error.status === 404) {
        return (<div>Not found requested data</div>);
    }

    if (error && 'status' in error && error.status === 401) {
        return (<div>Unauthorized</div>);
    }

    if (isLoading) {
        return (<div>Loading...</div>);
    }
}

export default ResponseInformation;