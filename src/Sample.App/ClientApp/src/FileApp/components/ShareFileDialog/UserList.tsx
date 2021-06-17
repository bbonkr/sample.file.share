import React, { useEffect, useState } from 'react';
import { UserModel } from '../../../api';

interface UserListProps {
    users: UserModel[];
    isLoading?: boolean;
    hasMore?: boolean;
    onMore?: () => void;
    onChangeSelectedUser?: (selected: UserModel[]) => void;
}

export const UserList = ({
    users,
    isLoading,
    hasMore,
    onMore,
    onChangeSelectedUser,
}: UserListProps) => {
    const [selectedUsers, setSelectedUsers] = useState<UserModel[]>([]);

    const handleChangeCheck =
        (user: UserModel) => (event: React.ChangeEvent<HTMLInputElement>) => {
            const checked = event.currentTarget.checked;
            setSelectedUsers((prevState) => {
                if (checked) {
                    prevState.push(user);
                } else {
                    const index = prevState.findIndex((x) => x.id === user.id);
                    if (index >= 0) {
                        prevState.splice(index, 1);
                    }
                }

                return [...prevState];
            });
        };

    const handleClickMore = () => {
        if (hasMore && onMore) {
            onMore();
        }
    };

    useEffect(() => {
        if (onChangeSelectedUser) {
            onChangeSelectedUser(selectedUsers);
        }
    }, [selectedUsers]);

    return (
        <ul>
            {users.length === 0 ? (
                <li>Search result is empty. Please try another keyword.</li>
            ) : (
                users.map((user) => (
                    <li className="mb-3" key={user.id}>
                        <label className="checkbox">
                            <input
                                type="checkbox"
                                onChange={handleChangeCheck(user)}
                                disabled={
                                    selectedUsers.length === 0
                                        ? false
                                        : selectedUsers.length > 0 &&
                                          selectedUsers.find(
                                              (_, index) => index === 0,
                                          )?.id !== user.id
                                }
                            />{' '}
                            {user.displayName}
                        </label>
                    </li>
                ))
            )}
            {users.length > 0 && hasMore && (
                <li className="mb-3">
                    <button
                        className="button"
                        disabled={isLoading}
                        onClick={handleClickMore}
                    >
                        Get more
                    </button>
                </li>
            )}
        </ul>
    );
};
