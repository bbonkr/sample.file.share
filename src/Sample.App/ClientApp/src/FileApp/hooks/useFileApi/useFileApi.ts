import { useSelector, useDispatch } from 'react-redux';
import {
    DeleteFileRequest,
    DeleteFileSharingRequest,
    GetFilesRequest,
    GetFilesSharedToMeRequest,
    ShareFileRequest,
    UploadFileRequest,
} from '../../models/files';
import { RootState, FileState, fileActions } from '../../store';

export const useFileApi = () => {
    const dispatch = useDispatch();
    const state = useSelector<RootState, FileState>((s) => s.file);

    return {
        ...state,
        getFilesRequest: (payload: GetFilesRequest) =>
            dispatch(fileActions.getFiles.request(payload)),
        uploadFilesRequest: (payload: UploadFileRequest) =>
            dispatch(fileActions.uploadFiles.request(payload)),
        shareFileRequest: (payload: ShareFileRequest) =>
            dispatch(fileActions.shareFile.request(payload)),
        deleteFileRequest: (payload: DeleteFileRequest) =>
            dispatch(fileActions.deleteFile.request(payload)),
        getFilesSharedToMeRequest: (payload: GetFilesSharedToMeRequest) =>
            dispatch(fileActions.getFilesSharedToMe.request(payload)),
        deleteFileSharingRequest: (payload: DeleteFileSharingRequest) =>
            dispatch(fileActions.deleteFileSharing.request(payload)),
        clearFiles: () => dispatch(fileActions.clearFileList()),
        clearErrorRequest: () => dispatch(fileActions.clearError()),
    };
};

export type UseFileApi = ReturnType<typeof useFileApi>;
