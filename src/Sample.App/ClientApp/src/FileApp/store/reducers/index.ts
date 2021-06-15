import { combineReducers } from 'redux';
import { userState } from './user';
import { messagingState } from './messaging';

export const rootState = combineReducers({
    user: userState,
    messaging: messagingState,
});

export type RootState = ReturnType<typeof rootState>;
