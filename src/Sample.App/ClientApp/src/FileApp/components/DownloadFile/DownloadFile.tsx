import React, { useEffect } from 'react';
import { Helmet } from 'react-helmet-async';
import { useUserApi } from '../../hooks/useUserApi';
import { useFileApi } from '../../hooks/useFileApi';
import { AuthProvider } from '../AuthProvider';
import { Section, Content, Modal } from '../Layouts';
import { useMessaging } from '../../hooks/useMessaging';
import { useState } from 'react';
import { SharedFileModel } from '../../../api';
import Axios from 'axios';
import { appOptions } from '../../constants';
import { FileDownloadHelper } from '@bbon/filedownload';
import dayjs from 'dayjs';
import { StringFormatter } from '@bbon/formatter';

const DownloadFile = () => {
    const FILE_LIST_LIMIT = 10;

    const { user } = useUserApi();
    const { addMessage } = useMessaging();
    const {
        filesSharedToMe,
        hasMoreFilesSharedToMe,
        fileError,
        getFilesSharedToMeRequest,
        deleteFileSharingRequest,
        clearErrorRequest,
    } = useFileApi();

    const [fileListPage, setFileListPage] = useState(1);
    const [deleteConfirmationDialogOpen, setDeleteConfirmationDialogOpen] =
        useState(false);
    const [selectedFileSharing, setSelectedFileSharing] =
        useState<SharedFileModel>();

    const handleClickMore = () => {
        if (user && user.email) {
            getFilesSharedToMeRequest({
                xApiKey: user.email,
                page: fileListPage + 1,
                keyword: '',
                limit: FILE_LIST_LIMIT,
            });

            setFileListPage((prevState) => prevState + 1);
        }
    };

    const handleClickDownload = (item: SharedFileModel) => () => {
        if (+new Date(item.expiresOn ?? 0) < +new Date()) {
            addMessage({
                id: `${+new Date()}`,
                title: 'Notification',
                detail: 'This sharing was expired.',
                duration: 'long',
                color: 'is-info',
            });
        } else {
            if (user && user.email) {
                const instance = Axios.create({ responseType: 'blob' });
                const url = `${window.location.protocol}//${window.location.host}/api/${appOptions.apiVersion}/files/${item.token}`;

                instance
                    .get(url, {
                        headers: {
                            ...Axios.defaults.headers,
                            'x-api-key': user.email,
                        },
                    })
                    .then((res) => {
                        if (res) {
                            const helper = new FileDownloadHelper();
                            helper.download({
                                data: res.data,
                                filename: item.name ?? 'downloadedfile',
                                contentType:
                                    item.contentType ??
                                    'application/octet-stream',
                            });
                        }
                    })
                    .catch((err) => {
                        console.error(err);
                    })
                    .finally(() => {});
            }
        }
    };

    const handleOpenDeleteConfirmationDialog =
        (item: SharedFileModel) => () => {
            console.info('handleOpenDeleteConfirmationDialog', item);
            setSelectedFileSharing((_) => item);
            setDeleteConfirmationDialogOpen((_) => true);
        };

    const handleCloseDeleteConfirmationDialog = () => {
        setSelectedFileSharing((_) => undefined);
        setDeleteConfirmationDialogOpen((_) => false);
    };

    const handleDeleteFileSharing = () => {
        if (
            selectedFileSharing &&
            selectedFileSharing.id &&
            user &&
            user.email
        ) {
            deleteFileSharingRequest({
                id: selectedFileSharing.id,
                xApiKey: user.email,
            });

            handleCloseDeleteConfirmationDialog();
        }
    };

    useEffect(() => {
        if (user && user.email) {
            getFilesSharedToMeRequest({
                xApiKey: user.email,
                page: fileListPage,
                keyword: '',
                limit: FILE_LIST_LIMIT,
            });
        }
    }, []);

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
            <Helmet title="Shared files" />
            <div className="is-flex is-flex-direction-column is-prevent-height-100 p-header">
                <Section
                    title={`🌈 Welcome ${user?.displayName}`}
                    subtitle="Shared to me"
                    useHero
                    heroColor="is-info"
                />
                <Section>
                    <Content>
                        <table className="table">
                            <thead>
                                <tr>
                                    <th>{` `}</th>
                                    <th>Name</th>
                                    <th className="has-text-centered">
                                        Expires on
                                    </th>
                                    <th className="has-text-right">Size</th>
                                    <th className="has-text-centered">Type</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                                {filesSharedToMe.length > 0 ? (
                                    filesSharedToMe.map((f) => {
                                        const dayjsInstance = dayjs(
                                            new Date(f.expiresOn ?? 0),
                                        );

                                        const expired =
                                            dayjsInstance.toDate() <=
                                            new Date();
                                        const stringFormatter =
                                            new StringFormatter();

                                        return (
                                            <tr key={f.token}>
                                                <td>
                                                    {f.token && (
                                                        <button
                                                            className="button"
                                                            onClick={handleClickDownload(
                                                                f,
                                                            )}
                                                        >
                                                            Download
                                                        </button>
                                                    )}
                                                    {f.id && (
                                                        <button
                                                            className="button"
                                                            onClick={handleOpenDeleteConfirmationDialog(
                                                                f,
                                                            )}
                                                        >
                                                            Remove
                                                        </button>
                                                    )}
                                                </td>
                                                <td>{f.name}</td>
                                                <td className="has-text-centered">
                                                    {f.expiresOn
                                                        ? `${dayjsInstance.format(
                                                              'YYYY-MM-DD HH:mm',
                                                          )}`
                                                        : 'No limit'}
                                                </td>
                                                <td className="has-text-right">
                                                    {stringFormatter.fileSize(
                                                        f.size ?? 0,
                                                        stringFormatter.numberWithDelimiter,
                                                    )}
                                                </td>
                                                <td className="has-text-centered">
                                                    {f.contentType}
                                                </td>
                                                <td className="text-has-centered">
                                                    <span
                                                        className={`tag ${
                                                            expired
                                                                ? 'is-danger'
                                                                : 'is-success'
                                                        }`}
                                                    >
                                                        {expired
                                                            ? 'Expired'
                                                            : 'Valid'}
                                                    </span>
                                                </td>
                                            </tr>
                                        );
                                    })
                                ) : (
                                    <tr>
                                        <td
                                            colSpan={6}
                                            className="has-text-centered"
                                        >
                                            There is no file
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                            {filesSharedToMe.length > 0 &&
                                hasMoreFilesSharedToMe && (
                                    <tfoot>
                                        <tr>
                                            <td
                                                colSpan={6}
                                                className="has-text-centered"
                                            >
                                                <button
                                                    className="button"
                                                    onClick={handleClickMore}
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

            <Modal
                open={deleteConfirmationDialogOpen}
                title="Confirmation"
                body="Are you sure to remove file sharing?"
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
                            onClick={handleDeleteFileSharing}
                        >
                            Remove
                        </button>
                    </div>
                }
            />
        </AuthProvider>
    );
};

export default DownloadFile;
