import React from "react";

export default function ShortenLinkForm() {
    const urlPattern = `/(https://www\.|http://www\.|https://|http://)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?/[a-zA-Z0-9]{2,}|((https://www\.|http://www\.|https://|http://)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?)|(https://www\.|http://www\.|https://|http://)?[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}(\.[a-zA-Z0-9]{2,})?/g;`
    
    return <form method='post' action="/links">
        <label htmlFor='fullLink-to-shorten-input'>Сократить ссылку:</label>
        <input name='fullUrl' type='text' id='fullLink-to-shorten-input' pattern="https?://[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}(/.*)?"/>
        <input type="submit" value="Сократить"/>
    </form>
}