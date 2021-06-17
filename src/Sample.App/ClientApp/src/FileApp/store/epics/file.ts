import { combineEpics, Epic } from 'redux-observable';
import { isActionOf } from 'typesafe-actions';
import { from, of } from 'rxjs';
import { filter, map, switchMap, catchError, mergeMap } from 'rxjs/operators';
import { RootState } from '../reducers';
import { ApiClient } from '../../services';
import { RootAction, rootAction } from '../actions';
import { ApiResponseModel } from '../../../api';
import { AxiosError, AxiosResponse } from 'axios';

const getFilesEpic: Epic<RootAction, RootAction, RootState, ApiClient> = (
    action$,
    state$,
    api,
) =>
    action$.pipe(
        filter(isActionOf(rootAction.file.getFiles.request)),
        switchMap((action) => {
            const { page, limit, keyword, xApiKey } = action.payload;

            return from(
                api.files.apiv10FilesMyFiles(xApiKey, page, limit, keyword),
            ).pipe(
                map((value) => rootAction.file.getFiles.success(value.data)),
                catchError((error) =>
                    of(rootAction.file.getFiles.failure(error.data)),
                ),
            );
        }),
    );

const uploadFileEpic: Epic<RootAction, RootAction, RootState, ApiClient> = (
    action$,
    state$,
    api,
) =>
    action$.pipe(
        filter(isActionOf(rootAction.file.uploadFiles.request)),
        switchMap((action) => {
            const { xApiKey, files } = action.payload;

            return from(api.files.apiv10FilesUpload(xApiKey, files)).pipe(
                map((value) => rootAction.file.uploadFiles.success(value.data)),
                catchError((error) =>
                    of(rootAction.file.uploadFiles.failure(error.data)),
                ),
            );
        }),
    );

const shareFileEpic: Epic<RootAction, RootAction, RootState, ApiClient> = (
    action$,
    state$,
    api,
) =>
    action$.pipe(
        filter(isActionOf(rootAction.file.shareFile.request)),
        switchMap((action) => {
            const { xApiKey, fileId, generateTokenRequest } = action.payload;

            return from(
                api.files.apiv10FilesGenerateToken(
                    fileId,
                    xApiKey,
                    generateTokenRequest,
                ),
            ).pipe(
                map((value) => rootAction.file.shareFile.success(value.data)),
                catchError((error) =>
                    of(rootAction.file.shareFile.failure(error.data)),
                ),
            );
        }),
    );

const deleteFileEpic: Epic<RootAction, RootAction, RootState, ApiClient> = (
    action$,
    state$,
    api,
) =>
    action$.pipe(
        filter(isActionOf(rootAction.file.deleteFile.request)),
        switchMap((action) => {
            const { xApiKey, fileId } = action.payload;

            return from(api.files.apiv10FilesDelete(fileId, xApiKey)).pipe(
                map((value) =>
                    rootAction.file.deleteFile.success({
                        ...value.data,
                        fileId: fileId,
                    }),
                ),
                catchError((error) => {
                    return of(rootAction.file.deleteFile.failure(error.data));
                }),
            );
        }),
    );

const getFilesSharedToMeEpic: Epic<
    RootAction,
    RootAction,
    RootState,
    ApiClient
> = (action$, state$, api) =>
    action$.pipe(
        filter(isActionOf(rootAction.file.getFilesSharedToMe.request)),
        switchMap((action) => {
            const { page, limit, keyword, xApiKey } = action.payload;

            return from(
                api.files.apiv10FilesSharedToMe(xApiKey, page, limit, keyword),
            ).pipe(
                map((value) =>
                    rootAction.file.getFilesSharedToMe.success(value.data),
                ),
                catchError((error) =>
                    of(rootAction.file.getFilesSharedToMe.failure(error.data)),
                ),
            );
        }),
    );

const deleteFileSharingEpic: Epic<
    RootAction,
    RootAction,
    RootState,
    ApiClient
> = (action$, state$, api) =>
    action$.pipe(
        filter(isActionOf(rootAction.file.deleteFileSharing.request)),
        switchMap((action) => {
            const { xApiKey, id } = action.payload;

            return from(api.files.apiv10FilesDeleteSharing(id, xApiKey)).pipe(
                map((value) =>
                    rootAction.file.deleteFileSharing.success({
                        ...value.data,
                        id: id,
                    }),
                ),
                catchError((error) =>
                    of(rootAction.file.deleteFileSharing.failure(error.data)),
                ),
            );
        }),
    );

export const fileEpic = combineEpics(
    getFilesEpic,
    uploadFileEpic,
    shareFileEpic,
    deleteFileEpic,
    getFilesSharedToMeEpic,
    deleteFileSharingEpic,
);

export type FileEpic = ReturnType<typeof fileEpic>;
