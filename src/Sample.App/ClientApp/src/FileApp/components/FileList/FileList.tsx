import React, { useEffect, useState } from 'react';
import { Helmet } from 'react-helmet-async';
import { useUserApi } from '../../hooks/useUserApi';
import { useFileApi } from '../../hooks/useFileApi';
import { AuthProvider } from '../AuthProvider';
import { Section, Content } from '../Layouts';
import { FileUpload } from './FileUpload';
import { FileItemModel, UserModel } from '../../../api';
import { ShareFileDialog } from '../ShareFileDialog';
import { useMessaging } from '../../hooks/useMessaging';
import { Modal } from '../Layouts';

const FileList = () => {
    const FILE_LIST_LIMIT = 10;
    const { user, clearUsersRequest } = useUserApi();
    const { addMessage } = useMessaging();
    const {
        files,
        hasMoreFiles,
        isLoadingFiles,
        fileError,
        getFilesRequest,
        uploadFilesRequest,
        deleteFileRequest,
        shareFileRequest,
        fileShareResult,
        clearErrorRequest,
    } = useFileApi();
    const [fileListPage, setFileListPage] = useState(1);
    const [shareFileDialogOpen, setShareFileDialogOpen] = useState(false);
    const [deleteConfirmationDialogOpen, setDeleteConfirmationDialogOpen] =
        useState(false);
    const [selectedFile, setSelectedFile] =
        useState<FileItemModel | null>(null);

    const handleUploadFiles = (files: File[]) => {
        if (user && user.email && files && files.length > 0) {
            uploadFilesRequest({ xApiKey: user.email, files: files });
        }
    };

    const handleOpenDeleteConfirmationDialog = (file: FileItemModel) => () => {
        setSelectedFile((_) => file);
        setDeleteConfirmationDialogOpen((_) => true);
    };

    const handleDeleteFile = () => {
        if (user && user.email && selectedFile?.id) {
            deleteFileRequest({
                xApiKey: user.email,
                fileId: selectedFile.id,
            });

            handleCloseDeleteConfirmationDialog();
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

    const handleCloseDeleteConfirmationDialog = () => {
        setSelectedFile((_) => null);
        setDeleteConfirmationDialogOpen((_) => false);
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

                    handleCloseShareFileDialog();
                }
            }
        }
    };

    const handleClickGetMore = () => {
        if (user && user.email) {
            getFilesRequest({
                xApiKey: user.email,
                page: fileListPage + 1,
                keyword: '',
                limit: FILE_LIST_LIMIT,
            });
            setFileListPage((prevState) => prevState + 1);
        }
    };

    useEffect(() => {
        if (user && user.email) {
            getFilesRequest({
                xApiKey: user.email,
                page: fileListPage,
                keyword: '',
                limit: FILE_LIST_LIMIT,
            });
        }
    }, []);

    useEffect(() => {
        if (!isLoadingFiles && fileShareResult) {
            addMessage({
                id: `${+new Date()}`,
                title: 'Notification',
                detail: `Token is ${fileShareResult.token}`,
                duration: 'long',
                color: 'is-success',
            });
        }
    }, [fileShareResult, isLoadingFiles]);

    useEffect(() => {
        if (fileError) {
            addMessage({
                id: `${+new Date()}`,
                title: 'Alert',
                detail: fileError.message,
                duration: 'long',
                color: 'is-danger',
            });
            clearErrorRequest();
        }
    }, [fileError]);

    return (
        <AuthProvider>
            <Helmet title="My files" />
            <div className="is-flex is-flex-direction-column is-prevent-height-100 p-header">
                <Section
                    title={`🌈 Welcome ${user?.displayName}`}
                    subtitle="My files"
                    useHero
                    heroColor="is-success"
                />
                <Section>
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
                                {files.length > 0 ? (
                                    files.map((f) => {
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
                                                            onClick={handleOpenDeleteConfirmationDialog(
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
                                    })
                                ) : (
                                    <tr>
                                        <td
                                            colSpan={5}
                                            className="has-text-centered"
                                        >
                                            Threr is no file
                                        </td>
                                    </tr>
                                )}
                            </tbody>

                            {files.length > 0 && hasMoreFiles && (
                                <tfoot>
                                    <tr>
                                        <td
                                            colSpan={5}
                                            className="has-text-centered"
                                        >
                                            <button
                                                className="button"
                                                onClick={handleClickGetMore}
                                            >
                                                Get more
                                            </button>
                                        </td>
                                    </tr>
                                </tfoot>
                            )}
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
            <Modal
                open={deleteConfirmationDialogOpen}
                title="Confirmation"
                body="Are you sure to delete file?"
                onClose={handleCloseDeleteConfirmationDialog}
                footer={
                    <div className="is-grouped">
                        <button
                            className="button"
                            onClick={handleCloseDeleteConfirmationDialog}
                        >
                            Cancel
                        </button>
                        <button
                            className="button is-danger"
                            onClick={handleDeleteFile}
                        >
                            Delete
                        </button>
                    </div>
                }
            />
        </AuthProvider>
    );
};

export default FileList;
