import React, { useEffect, useMemo, useState } from 'react';
import throttle from 'lodash/throttle';
import { useUserApi } from '../../hooks/useUserApi';
import { Modal } from '../Layouts';
import { UserSearchForm } from './UserSearchForm';
import { UserList } from './UserList';
import { ExpirationForm } from './ExpirationForm';
import { FileItemModel, UserModel } from '../../../api';

interface ShareFileDialogProps {
    file?: FileItemModel | null;
    open?: boolean;
    onShare?: (
        file: FileItemModel,
        expiresOn: number,
        users: UserModel[],
    ) => void;
    onClose?: () => void;
}

export const ShareFileDialog = ({
    file,
    open,
    onShare,
    onClose,
}: ShareFileDialogProps) => {
    const USER_LIST_LIMIT = 10;
    const { user, users, getUsersRequest, hasMoreGetUsers, isLoadingUsers } =
        useUserApi();

    const [keyword, setKeyword] = useState('');
    const [userListPage, setUserListPage] = useState(1);
    const [expiresOn, setExpiresOn] = useState<number>();
    const [selectedUsers, setSelectedUsers] = useState<UserModel[]>([]);

    const handleChangeSearchKeyword = throttle((keyword: string) => {
        if (!isLoadingUsers && keyword && user && user.email) {
            getUsersRequest({
                xApiKey: user.email,
                keyword: keyword,
                page: 1,
                limit: USER_LIST_LIMIT,
            });
            setKeyword((_) => keyword);
            setUserListPage((_) => 1);
        }
    }, 200);

    const handleChangeSelectedUser = (users: UserModel[]) => {
        setSelectedUsers((_) => users);
    };

    const handleChangeTopic = (time?: number) => {
        setExpiresOn((_) => time);
    };

    const handleClickShare = () => {
        if (onShare) {
            if (file && expiresOn && selectedUsers.length > 0) {
                onShare(file, expiresOn, selectedUsers);
            }
        }
    };

    const handleMore = () => {
        if (!isLoadingUsers && keyword) {
            if (!isLoadingUsers && keyword) {
                getUsersRequest({
                    keyword: keyword,
                    page: userListPage,
                    limit: USER_LIST_LIMIT,
                });

                setKeyword((_) => keyword);
                setUserListPage((prevState) => prevState + 1);
            }
        }
    };

    return (
        <Modal
            title="Thread"
            open={open}
            onClose={onClose}
            footer={
                <div className="field is-grouped">
                    <button
                        className="button is-primary"
                        disabled={!expiresOn || selectedUsers.length === 0}
                        onClick={handleClickShare}
                    >
                        Share
                    </button>
                    <button className="button" onClick={onClose}>
                        Cancel
                    </button>
                </div>
            }
        >
            <p className="title">Share to</p>

            <div className="p-3">
                <ExpirationForm onChange={handleChangeTopic} />
            </div>

            <div className="p-3">
                <UserSearchForm onSearch={handleChangeSearchKeyword} />
            </div>
            <div className="p-3">
                <UserList
                    users={users.filter((x) => x.id !== user?.id)}
                    hasMore={hasMoreGetUsers}
                    isLoading={isLoadingUsers}
                    onChangeSelectedUser={handleChangeSelectedUser}
                    onMore={handleMore}
                />
            </div>
        </Modal>
    );
};
