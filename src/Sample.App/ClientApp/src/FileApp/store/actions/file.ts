import { ActionType, createAction, createAsyncAction } from 'typesafe-actions';
import {
    ApiResponseModel,
    FileItemModelIPagedModelApiResponseModel,
    ShareFileResultApiResponseModel,
    FileItemModelIListApiResponseModel,
    SharedFileModelIPagedModelApiResponseModel,
} from '../../../api';
import {
    DeleteFileRequest,
    DeleteFileResponse,
    DeleteFileSharingApiResponseModel,
    DeleteFileSharingRequest,
    GetFilesRequest,
    GetFilesSharedToMeRequest,
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

const getFilesSharedToMe = createAsyncAction(
    'get-files-shared-to-me/request',
    'get-files-shared-to-me/success',
    'get-files-shared-to-me/failure',
)<
    GetFilesSharedToMeRequest,
    SharedFileModelIPagedModelApiResponseModel,
    ApiResponseModel
>();

const deleteFileSharing = createAsyncAction(
    'delete-file-sharing/request',
    'delete-file-sharing/success',
    'delete-file-sharing/failure',
)<
    DeleteFileSharingRequest,
    DeleteFileSharingApiResponseModel,
    ApiResponseModel
>();

const clearFileList = createAction('clear-file-list')();
const clearError = createAction('clear-error')();

export const fileActions = {
    getFiles,
    uploadFiles,
    shareFile,
    deleteFile,
    getFilesSharedToMe,
    deleteFileSharing,
    clearFileList,
    clearError,
};

export type FileActions = ActionType<typeof fileActions>;
