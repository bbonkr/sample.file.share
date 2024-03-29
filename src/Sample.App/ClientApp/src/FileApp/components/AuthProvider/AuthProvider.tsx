import React, { PropsWithChildren, useEffect } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import { useUserApi } from '../../hooks/useUserApi';

interface AuthProviderProps {}

export const AuthProvider = ({
    children,
}: PropsWithChildren<AuthProviderProps>) => {
    const history = useHistory();
    const location = useLocation();
    const { user, isLoadingUser } = useUserApi();

    useEffect(() => {
        if (!user && !isLoadingUser) {
            history.replace(
                `/signin?returnUrl=${encodeURIComponent(
                    `${location.pathname}${location.search}`,
                )}`,
            );
        }
    }, [user]);

    return <React.Fragment>{children}</React.Fragment>;
};
