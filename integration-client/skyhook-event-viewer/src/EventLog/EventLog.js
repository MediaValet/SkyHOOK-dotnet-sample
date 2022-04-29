import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';

import EventLogWindow from './EventLogWindow/EventLogWindow';

const EventLog = () => {
    const [ eventLog, setEventLog ] = useState([]);
    const latestEventLog = useRef(null);

    latestEventLog.current = eventLog;

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/hubs/cloudevents')
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(result => {
                console.log('Connected!');

                connection.on('ReceiveCloudEvent', message => {
                    const updatedEventLog = [...latestEventLog.current];
                    updatedEventLog.push(message);
                
                    setEventLog(updatedEventLog);
                });
            })
            .catch(e => console.log('Connection failed: ', e));
    }, []);

    return (
        <div>
            <EventLogWindow eventLog={eventLog}/>
        </div>
    );
};

export default EventLog;