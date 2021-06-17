import { combineReducers } from 'redux';
import { userState } from './user';
import { messagingState } from './messaging';
import { fileState } from './file';

export * from './file';
export * from './messaging';
export * from './user';

export const rootState = combineReducers({
    user: userState,
    messaging: messagingState,
    file: fileState,
});

export type RootState = ReturnType<typeof rootState>;
