import React, { useState, useEffect } from 'react';
import { getResources } from '../services/Api'

function HomePage() {
    const [data, setData] = useState("default");
    const [first, setFirst] = useState("default");
    const [second, setSecond] = useState("default");
    const [third, setThird] = useState("default");
    
    useEffect(() => {
        async function prefetch() {
            const response = await getResources();
            console.log(response);
            setData(response);
        }

        prefetch();
    });

    async function sendRequests() {
        getResources().then(data => {
            console.log('first ' + data)
            setFirst(data)
        }).catch(err => {
            console.log('error on first req' + err)
        });

        getResources().then(data => {
            console.log('second ' + data)
            setSecond(data)
        }).catch(err => {
            console.log('error on second req' + err)
        });

        getResources().then(data => {
            console.log('third ' + data)
            setThird(data)
        }).catch(err => {
            console.log('error on third req' + err)
        });
    }

    return (
        <div>
            <button onClick={sendRequests}>Send 3 requests</button>
            Home Page {data}
            <br />
            <br />

            First resp: {first}

            <br />

            Second resp: {second}

            <br />
            Third resp: {third}
        </div>
    );
}
export default HomePage;