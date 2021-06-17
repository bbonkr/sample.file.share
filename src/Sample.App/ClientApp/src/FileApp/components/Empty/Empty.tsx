import React from 'react';
import { AuthProvider } from '../AuthProvider';

export const Empty = () => <div>Hello, world!</div>;

export const EmptyRequiredAuth = () => (
    <AuthProvider>
        <div>Hello, world!</div>
    </AuthProvider>
);
