import { ConsoleLogger } from '@microsoft/signalr/dist/esm/Utils';
import React from 'react';

import Message from './AssetMediaFileAddedEvent/AssetMediaFileAddedEvent';

const EventLogWindow = (props) => {
    const eventLog = props.eventLog
        .map(m => <Message 
            key={Date.now() * Math.random()}
            type={m.type}
            time={m.time}
            filename={m.data.fileName}
            imageUrl={m.data.mediaFileBlobUrl}/>);

    return(
        <div>
            {eventLog}
        </div>
    )
};

export default EventLogWindow;