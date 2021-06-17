import { combineEpics, Epic } from 'redux-observable';
import { isActionOf } from 'typesafe-actions';
import { from, of } from 'rxjs';
import { filter, map, switchMap, catchError, mergeMap } from 'rxjs/operators';
import { RootState } from '../reducers';
import { ApiClient } from '../../services';
import { RootAction, rootAction } from '../actions';

const findUserByEmailEpic: Epic<RootAction, RootAction, RootState, ApiClient> =
    (action$, state$, api) =>
        action$.pipe(
            filter(isActionOf(rootAction.user.getUserByEmail.request)),
            switchMap((action) => {
                const { email } = action.payload;

                return from(api.users.apiv10UsersFindByEmail(email)).pipe(
                    map((value) =>
                        rootAction.user.getUserByEmail.success(value.data),
                    ),
                    catchError((error) =>
                        of(rootAction.user.getUserByEmail.failure(error.data)),
                    ),
                );
            }),
        );

const createUserEpic: Epic<RootAction, RootAction, RootState, ApiClient> = (
    action$,
    state$,
    api,
) =>
    action$.pipe(
        filter(isActionOf(rootAction.user.createUser.request)),
        switchMap((action) => {
            return from(api.users.apiv10UsersCreate(action.payload)).pipe(
                mergeMap((value) =>
                    of(rootAction.user.createUser.success(value.data)),
                ),
                catchError((error) => {
                    return of(rootAction.user.createUser.failure(error.data));
                }),
            );
        }),
    );

const deleteUserEpic: Epic<RootAction, RootAction, RootState, ApiClient> = (
    action$,
    state$,
    api,
) =>
    action$.pipe(
        filter(isActionOf(rootAction.user.deleteUser.request)),
        switchMap((action) => {
            const { email } = action.payload;
            return from(api.users.apiv10UsersDelete(email)).pipe(
                map((value) => rootAction.user.deleteUser.success(value.data)),
                catchError((error) =>
                    of(rootAction.user.deleteUser.failure(error.data)),
                ),
            );
        }),
    );

const getUsersEpic: Epic<RootAction, RootAction, RootState, ApiClient> = (
    action$,
    state$,
    api,
) =>
    action$.pipe(
        filter(isActionOf(rootAction.user.getUsers.request)),
        switchMap((action) => {
            const { xApiKey, page, limit, keyword } = action.payload;
            return from(
                api.users.apiv10UsersGetUsers(xApiKey, page, limit, keyword),
            ).pipe(
                map((value) => rootAction.user.getUsers.success(value.data)),
                catchError((error) =>
                    of(rootAction.user.getUsers.failure(error.data)),
                ),
            );
        }),
    );

export const userEpic = combineEpics(
    findUserByEmailEpic,
    createUserEpic,
    deleteUserEpic,
    getUsersEpic,
);

export type UserEpic = ReturnType<typeof userEpic>;
