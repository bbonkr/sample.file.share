import { combineEpics } from 'redux-observable';
import { userEpic } from './user';
import { fileEpic } from './file';

export const rootEpic = combineEpics(userEpic, fileEpic);

export type RootEpic = ReturnType<typeof rootEpic>;
