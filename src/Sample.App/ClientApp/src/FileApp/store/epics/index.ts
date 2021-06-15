import { combineEpics } from 'redux-observable';
import { userEpic } from './user';

export const rootEpic = combineEpics(userEpic);

export type RootEpic = ReturnType<typeof rootEpic>;
