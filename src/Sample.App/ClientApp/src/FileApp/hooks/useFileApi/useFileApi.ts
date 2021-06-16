import { useSelector, useDispatch } from 'react-redux';
import {
    DeleteFileRequest,
    GetFilesRequest,
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
        clearFiles: () => dispatch(fileActions.clearFileList()),
        clearErrorRequest: () => dispatch(fileActions.clearError()),
    };
};

export type UseFileApi = ReturnType<typeof useFileApi>;
