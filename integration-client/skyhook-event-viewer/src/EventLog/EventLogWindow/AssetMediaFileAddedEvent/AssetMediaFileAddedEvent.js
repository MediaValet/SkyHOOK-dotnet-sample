import React from 'react';

const AssetMediaFileAddedEvent = (props) => (
    <div style={{ background: "#eee", borderRadius: '5px', padding: '0 10px' }}>
        <p>{props.time} </p>
        <p><strong>{props.type}:</strong> {props.filename}</p>
    </div>
);

export default AssetMediaFileAddedEvent;