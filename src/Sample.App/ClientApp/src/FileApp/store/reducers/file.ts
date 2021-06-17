import { combineReducers } from 'redux';
import { createReducer } from 'typesafe-actions';
import {
    ApiResponseModel,
    FileItemModel,
    SharedFileModel,
    ShareFileResult,
} from '../../../api';
import { FileActions, fileActions } from '../actions/file';

const files = createReducer<FileItemModel[], FileActions>([])
    .handleAction([fileActions.getFiles.success], (state, action) => {
        const page = action.payload.data?.currentPage ?? 1;
        const list = action.payload.data?.items ?? [];
        if (page === 1) {
            return list;
        } else {
            return [...state, ...list];
        }
    })
    .handleAction([fileActions.uploadFiles.success], (state, action) => {
        const list = action.payload.data ?? [];

        return [...list, ...state];
    })
    .handleAction([fileActions.deleteFile.success], (state, action) => {
        const index = state.findIndex((x) => x.id === action.payload.fileId);
        if (index >= 0) {
            state.splice(index, 1);
            return [...state];
        }
        return state;
    })
    .handleAction([fileActions.clearFileList], (_, __) => []);

const isLoadingFiles = createReducer<boolean, FileActions>(false)
    .handleAction(
        [
            fileActions.getFiles.request,
            fileActions.deleteFile.request,
            fileActions.shareFile.request,
            fileActions.uploadFiles.request,
        ],
        (_, __) => true,
    )
    .handleAction(
        [
            fileActions.getFiles.success,
            fileActions.getFiles.failure,
            fileActions.deleteFile.success,
            fileActions.deleteFile.failure,
            fileActions.shareFile.success,
            fileActions.shareFile.failure,
            fileActions.uploadFiles.success,
            fileActions.uploadFiles.failure,
        ],
        (_, __) => false,
    );

const hasMoreFiles = createReducer<boolean, FileActions>(true)
    .handleAction(
        [fileActions.getFiles.request, fileActions.getFiles.failure],
        (_, __) => true,
    )
    .handleAction([fileActions.getFiles.success], (state, action) => {
        const limit = action.payload.data?.limit ?? 0;
        const items = action.payload.data?.items?.length ?? 0;

        if (items === 0) {
            return false;
        }
        if (limit === 0) {
            return true;
        }
        return limit === items;
    });

const fileError = createReducer<ApiResponseModel | null, FileActions>(null)
    .handleAction(
        [
            fileActions.getFiles.failure,
            fileActions.shareFile.failure,
            fileActions.uploadFiles.failure,
            fileActions.deleteFile.failure,
            fileActions.getFilesSharedToMe.failure,
        ],
        (state, action) => action.payload ?? null,
    )
    .handleAction(
        [
            fileActions.getFiles.request,
            fileActions.getFiles.success,
            fileActions.shareFile.request,
            fileActions.shareFile.success,
            fileActions.uploadFiles.request,
            fileActions.uploadFiles.success,
            fileActions.deleteFile.request,
            fileActions.deleteFile.success,
            fileActions.getFilesSharedToMe.request,
            fileActions.getFilesSharedToMe.success,
            fileActions.clearError,
        ],
        (_, __) => null,
    );

const fileShareResult = createReducer<ShareFileResult | null, FileActions>(null)
    .handleAction(
        [fileActions.shareFile.success],
        (state, action) => action.payload.data ?? null,
    )
    .handleAction(
        [
            fileActions.shareFile.request,
            fileActions.shareFile.failure,
            fileActions.getFiles.request,
            fileActions.shareFile.request,
            fileActions.uploadFiles.request,
            fileActions.deleteFile.request,
        ],
        (_, __) => null,
    );

const filesSharedToMe = createReducer<SharedFileModel[], FileActions>([])
    .handleAction([fileActions.getFilesSharedToMe.success], (state, action) => {
        const page = action.payload.data?.currentPage ?? 1;
        const list = action.payload.data?.items ?? [];
        if (page === 1) {
            return list;
        } else {
            return [...state, ...list];
        }
    })
    .handleAction([fileActions.deleteFileSharing.success], (state, action) => {
        const index = state.findIndex((x) => x.id === action.payload.id);
        if (index >= 0) {
            state.splice(index, 1);
        }

        return [...state];
    });

const hasMoreFilesSharedToMe = createReducer<boolean, FileActions>(true)
    .handleAction(
        [
            fileActions.getFilesSharedToMe.request,
            fileActions.getFilesSharedToMe.failure,
        ],
        (_, __) => true,
    )
    .handleAction([fileActions.getFilesSharedToMe.success], (state, action) => {
        const limit = action.payload.data?.limit ?? 0;
        const items = action.payload.data?.items?.length ?? 0;

        if (items === 0) {
            return false;
        }
        if (limit === 0) {
            return true;
        }
        return limit === items;
    });

export const fileState = combineReducers({
    files,
    isLoadingFiles,
    hasMoreFiles,
    fileShareResult,
    fileError,
    filesSharedToMe,
    hasMoreFilesSharedToMe,
});

export type FileState = ReturnType<typeof fileState>;
