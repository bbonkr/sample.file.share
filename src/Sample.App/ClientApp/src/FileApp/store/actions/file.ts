import { ActionType, createAction, createAsyncAction } from 'typesafe-actions';
import {
    ApiResponseModel,
    FileItemModelIPagedModelApiResponseModel,
    ShareFileResultApiResponseModel,
    FileItemModelIListApiResponseModel,
} from '../../../api';
import {
    DeleteFileRequest,
    DeleteFileResponse,
    GetFilesRequest,
    ShareFileRequest,
    UploadFileRequest,
} from '../../models/files';

const getFiles = createAsyncAction(
    'get-files/request',
    'get-files/success',
    'get-files/failure',
)<
    GetFilesRequest,
    FileItemModelIPagedModelApiResponseModel,
    ApiResponseModel
>();

const uploadFiles = createAsyncAction(
    'upload-files/request',
    'upload-files/success',
    'upload-files/failure',
)<UploadFileRequest, FileItemModelIListApiResponseModel, ApiResponseModel>();

const shareFile = createAsyncAction(
    'share-file/request',
    'share-file/success',
    'share-file/failure',
)<ShareFileRequest, ShareFileResultApiResponseModel, ApiResponseModel>();

const deleteFile = createAsyncAction(
    'delete-file/request',
    'delete-file/success',
    'delete-file/failure',
)<DeleteFileRequest, DeleteFileResponse, ApiResponseModel>();

const clearFileList = createAction('clear-file-list')();
const clearError = createAction('clear-error')();

export const fileActions = {
    getFiles,
    uploadFiles,
    shareFile,
    deleteFile,
    clearFileList,
    clearError,
};

export type FileActions = ActionType<typeof fileActions>;
