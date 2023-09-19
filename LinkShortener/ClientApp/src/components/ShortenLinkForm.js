import React from "react";

export default function ShortenLinkForm() {
    const onSubmit = (event) => {
        event.preventDefault();
        const input = document.getElementById('fullLink-to-shorten-input');
        const fullUrl = input.value; 
        fetch(`/links`,
            {
                method: 'post',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(fullUrl)
            })
            .then(res => {
                if (res.ok) {
                    alert('Created');
                    window.location.reload();
                } else alert(`status code: ${res.status}`)
            })
    };
    
    return <form id="new-short-url" onSubmit={onSubmit}>
        <label className="m-1" htmlFor='fullLink-to-shorten-input'>Сократить ссылку:</label>
        <input className="m-1" name='fullUrl' type='url' id='fullLink-to-shorten-input' 
               required />
        <input className="mx-2 my-1" type="submit" value="Сократить"/>
    </form>
}