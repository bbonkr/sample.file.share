import React, { useEffect, useState } from 'react';
import dayjs from 'dayjs';

interface ExpirationFormProps {
    onChange?: (expiresOn?: number) => void;
}

export const ExpirationForm = ({ onChange }: ExpirationFormProps) => {
    const [time, setTime] = useState('');
    const [date, setDate] = useState('');
    const [dateTime, setDateTime] = useState(0);

    const handleClickReset = () => {
        setDate((_) => '');
        setTime((_) => '');
    };

    const handleChangeTopic = (event: React.ChangeEvent<HTMLInputElement>) => {
        var name = event.currentTarget.name;
        const value = event.currentTarget.value;
        console.info('time value', value);

        switch (name) {
            case 'date':
                setDate((_) => value);
                break;
            case 'time':
                setTime((_) => value);
                break;
        }
    };

    useEffect(() => {
        if (onChange) {
            if (date && time) {
                console.info(
                    'Expiration date, time',
                    date,
                    time,
                    dayjs(`${date} ${time || '00:00'}`).unix() * 1000,
                    new Date(
                        dayjs(`${date} ${time || '00:00'}`).unix() * 1000,
                    ).toISOString(),
                );
                const expiresOn =
                    dayjs(`${date} ${time || '00:00'}`).unix() * 1000;

                onChange(expiresOn);
            } else {
                onChange(undefined);
            }
        }
    }, [date, time]);

    return (
        <div className="field is-horizontal">
            <div className="field-label is-normal">
                <label className="label">Expiration</label>
            </div>
            <div className="field-body">
                <div className="field">
                    {' '}
                    <div className="control">
                        <input
                            type="date"
                            className="input"
                            name="date"
                            value={date}
                            onChange={handleChangeTopic}
                            min={dayjs().format('YYYY-MM-DD')}
                        />
                    </div>
                </div>
                <div className="field">
                    <div className="control">
                        <input
                            type="time"
                            className="input"
                            name="time"
                            value={time}
                            onChange={handleChangeTopic}
                        />
                    </div>
                </div>
                <div className="field">
                    <div className="control">
                        <button className="button" onClick={handleClickReset}>
                            Reset
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};
