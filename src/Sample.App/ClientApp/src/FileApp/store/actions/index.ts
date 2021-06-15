import { ActionType } from 'typesafe-actions';
import { userActions } from './user';
import { messagingActions } from './messaging';

export const rootAction = {
    user: userActions,
    messaging: messagingActions,
};

export type RootAction = ActionType<typeof rootAction>;
