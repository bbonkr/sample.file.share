import React from 'react';
import { FaCloudUploadAlt } from 'react-icons/fa';

interface FileUploadProps {
    isLoading?: boolean;
    onUplad?: (files: File[]) => void;
}

export const FileUpload = ({ isLoading, onUplad }: FileUploadProps) => {
    const handleClickChangeFile = (
        event: React.ChangeEvent<HTMLInputElement>,
    ) => {
        if (event.currentTarget.files) {
            const files = Array.from(event.currentTarget.files).map(
                (f: File) => f,
            );
            if (onUplad) {
                onUplad(files);
            }

            event.currentTarget.value = '';
        }
    };

    return (
        <form>
            <div className="filed">
                <div className="control">
                    <div className="file">
                        <label className="file-label">
                            <input
                                className="file-input"
                                type="file"
                                name="files"
                                multiple
                                onChange={handleClickChangeFile}
                                disabled={isLoading}
                            />

                            <span className="file-cta">
                                <span className="file-icon">
                                    <FaCloudUploadAlt />
                                </span>
                                <span className="file-label">Upload files</span>
                            </span>
                        </label>
                    </div>
                </div>
            </div>
        </form>
    );
};
