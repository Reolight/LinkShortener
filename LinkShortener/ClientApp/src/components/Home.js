import React, {useEffect, useState} from 'react';
import ShortenLinkForm from "./ShortenLinkForm";
import { Redirect } from 'react-router-dom'

export function Home() {
    const [data, setData] = useState({ isLoading: true });
    
    useEffect(() => {
        if (data.isLoading)
        {
            fetch('/links').then(response => {
                if (response.ok)
                {
                    response.json().then(loaded => 
                        setData({ isLoading: false, urls: loaded} )
                    );
                }
            });
        }
        
    }, [data.isLoading])

    const handleRemove = (shortUrl) => {
        fetch(`/links/${shortUrl}`, { method: 'delete'})
            .then(res => {
                if (res.ok)
                    setData({isLoading: true });
                else res.json().then(data=>console.error(data));
            }            
        );
    }
    
    if (data.isLoading) return <p>Loading...</p>
    if (!data.urls.length) return <div>
        <i>Пока нет сокращенных ссылок</i>
        <ShortenLinkForm />
    </div>
    
    return (
      <div>
        <table>
            <thead>
                <tr>
                    <th>No.</th>
                    <th>Короткая ссылка</th>
                    <th>Полная ссылка</th>
                    <th>Кол-во переходов</th>
                    <th>Дата создания</th>
                    <th/>
                </tr>
            </thead>
            <tbody>
                {data.urls.map((url, index) => {
                    const shortenedUrl = `${window.location.origin}/${url.shortUrl}`;
                    return(<tr key={index}>
                        <td>{index+1}</td>
                        <td><a href={shortenedUrl}>{shortenedUrl}</a></td>
                        <td><a href={url.fullUrl}>{url.fullUrl}</a></td>
                        <td>{url.visitedTimes}</td>
                        <td>{url.creationTime}</td>
                        <td className="control-td-row">
                            <input type="button" value="X" onClick={()=> handleRemove(url.shortUrl)}/>
                            <input type="button" value="V" />
                        </td>
                    </tr>)
                })}
            </tbody>
        </table>
          
          <ShortenLinkForm/>
      </div>
    );
}