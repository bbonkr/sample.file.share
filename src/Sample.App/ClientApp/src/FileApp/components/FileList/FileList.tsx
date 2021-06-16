import React, { useEffect, useState } from 'react';
import { useUserApi } from '../../hooks/useUserApi';
import { useFileApi } from '../../hooks/useFileApi';
import { AuthProvider } from '../AuthProvider';
import { Section, Content } from '../Layouts';
import { FileUpload } from './FileUpload';
import { FileItemModel, UserModel } from '../../../api';
import { ShareFileDialog } from '../ShareFileDialog';
import { useMessaging } from '../../hooks/useMessaging';

const FileList = () => {
    const { user, clearUsersRequest } = useUserApi();
    const { addMessage } = useMessaging();
    const {
        files,
        isLoadingFiles,
        getFilesRequest,
        uploadFilesRequest,
        deleteFileRequest,
        shareFileRequest,
        fileShareResult,
    } = useFileApi();

    const [shareFileDialogOpen, setShareFileDialogOpen] = useState(false);
    const [selectedFile, setSelectedFile] =
        useState<FileItemModel | null>(null);

    const handleUploadFiles = (files: File[]) => {
        if (user && user.email && files && files.length > 0) {
            uploadFilesRequest({ xApiKey: user.email, files: files });
        }
    };

    const handleDeleteFile = (item: FileItemModel) => () => {
        if (user && user.email && item.id) {
            deleteFileRequest({
                xApiKey: user.email,
                fileId: item.id,
            });
        }
    };

    const handleOpenShareFileDialog = (item: FileItemModel) => () => {
        clearUsersRequest();
        setSelectedFile((_) => item);
        setShareFileDialogOpen((_) => true);
    };

    const handleCloseShareFileDialog = () => {
        setSelectedFile((_) => null);
        setShareFileDialogOpen((_) => false);
    };

    const handleShare = (
        file: FileItemModel,
        expiresOn: number,
        users: UserModel[],
    ) => {
        if (file && expiresOn && users.length > 0) {
            if (user && user.email && file.id) {
                const userId = users.find((u, index) => index === 0)?.id;
                if (userId) {
                    shareFileRequest({
                        xApiKey: user.email,
                        fileId: file.id,
                        generateTokenRequest: {
                            to: userId,
                            expiresOn: expiresOn,
                        },
                    });
                }
            }
        }
    };

    useEffect(() => {
        if (user && user.email) {
            getFilesRequest({
                xApiKey: user.email,
                page: 1,
                keyword: '',
                limit: 10,
            });
        }
    }, []);

    useEffect(() => {
        if (!isLoadingFiles && fileShareResult) {
            addMessage({
                id: `${+new Date()}`,
                title: 'Notification',
                detail: ``,
                duration: 'long',
                color: 'is-success',
            });
        }
    }, [fileShareResult, isLoadingFiles]);

    return (
        <AuthProvider>
            <div className="is-flex is-flex-direction-column is-prevent-height-100 p-header">
                <Section title={`🌈 Welcome ${user?.displayName}`}>
                    <Content>
                        <FileUpload
                            isLoading={isLoadingFiles}
                            onUplad={handleUploadFiles}
                        />

                        <table className="table">
                            <thead>
                                <tr>
                                    <th> </th>
                                    <th>Name</th>
                                    <th>Size</th>
                                    <th>Type</th>
                                    <th>Note</th>
                                </tr>
                            </thead>
                            <tbody>
                                {files.map((f) => {
                                    return (
                                        <tr
                                            key={f.id}
                                            className={`${
                                                selectedFile?.id === f.id
                                                    ? 'is-selected'
                                                    : ''
                                            }`}
                                        >
                                            <td>
                                                <div className="is-grouped">
                                                    <button
                                                        className="button"
                                                        onClick={handleOpenShareFileDialog(
                                                            f,
                                                        )}
                                                    >
                                                        Share
                                                    </button>
                                                    <button
                                                        className="button"
                                                        onClick={handleDeleteFile(
                                                            f,
                                                        )}
                                                    >
                                                        Delete
                                                    </button>
                                                </div>
                                            </td>
                                            <td>{f.name}</td>
                                            <td>{f.size}</td>
                                            <td>{f.contentType}</td>
                                            <td>
                                                {f.uri && (
                                                    <a
                                                        href={f.uri}
                                                        target="_blank"
                                                        className="button"
                                                    >
                                                        Download
                                                    </a>
                                                )}
                                            </td>
                                        </tr>
                                    );
                                })}
                            </tbody>
                        </table>
                    </Content>
                </Section>
            </div>
            <ShareFileDialog
                open={shareFileDialogOpen}
                file={selectedFile}
                onClose={handleCloseShareFileDialog}
                onShare={handleShare}
            />
        </AuthProvider>
    );
};

export default FileList;
