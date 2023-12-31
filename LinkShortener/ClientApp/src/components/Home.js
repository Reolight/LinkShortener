import React from 'react';
import ShortenLinkForm from "./ShortenLinkForm";
import { useLoaderData } from 'react-router-dom';

export function Home() {
    const urls = useLoaderData();

    const handleRemove = (shortUrl) => {
        fetch(`/links/${shortUrl}`, { method: 'delete'})
            .then(res => {
                if (res.ok)
                    window.location.reload();
                else res.json().then(data=>console.error(data));
            }            
        );
    }
    
    if (!urls.length) return <div>
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
                {urls.map((url, index) => {
                    const shortenedUrl = `${window.location.origin}/${url.shortUrl}`;
                    return(<tr key={index}>
                        <td>{index+1}</td>
                        <td><a href={shortenedUrl}>{shortenedUrl}</a></td>
                        <td><a href={url.fullUrl}>{url.fullUrl}</a></td>
                        <td>{url.visitedTimes}</td>
                        <td>{url.creationTime}</td>
                        <td className="control-td-row">
                            <input type="button" value="X" onClick={()=> handleRemove(url.shortUrl)}/>
                            <input type="button" value="V" onClick={()=> {
                                const shortUrl = url.shortUrl;
                                window.location.assign(`url/${shortUrl}`);
                            }} />
                        </td>
                    </tr>)
                })}
            </tbody>
        </table>
          
          <ShortenLinkForm/>
      </div>
    );
}