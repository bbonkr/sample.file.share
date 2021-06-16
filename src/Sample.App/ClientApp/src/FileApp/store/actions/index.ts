import { ActionType } from 'typesafe-actions';
import { userActions } from './user';
import { messagingActions } from './messaging';
import { fileActions } from './file';

export * from './file';
export * from './user';
export * from './messaging';

export const rootAction = {
    user: userActions,
    messaging: messagingActions,
    file: fileActions,
};

export type RootAction = ActionType<typeof rootAction>;
