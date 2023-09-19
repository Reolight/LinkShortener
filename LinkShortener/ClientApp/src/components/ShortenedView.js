import {useLoaderData} from "react-router-dom";
import {useState} from "react";

export default function ShortenedView(){
   const url = useLoaderData();
   
   const fullUrl = new URL(url.fullUrl);
   const shortRedirectionUrl = `${window.location.origin}/${url.shortUrl}`
   const [formState, setFormState] = useState({ currentUrlValue: url.fullUrl, isChanged: false });
   
   const handleUrlChange = (value) => {
       if (value !== formState.currentUrlValue && !formState.isChanged){
           setFormState({...formState, isChanged: true});
           return;
       }
       
       if (value === formState.currentUrlValue && formState.isChanged){
           setFormState({...formState, isChanged: false});
       }
       
       setFormState({...formState, currentUrlValue: value })
   };
   
   const handleSubmit = (e) => {
       e.preventDefault();
       fetch(`/links/${url.shortUrl}`, 
           {
               method: 'put',
               headers: { 'Content-Type': 'application/json' },
               body: JSON.stringify(formState.currentUrlValue) 
           })
           .then(res => {
               if (res.ok) {
                   alert('Accepted');
                   window.location.assign(window.location.origin);
               } else alert(`status code: ${res.status}`)
           })
       };
   
   return (<div className="flex-row mx-auto">
       <link className="m-1 w-25 h-25" href={`${fullUrl.origin}/favicon.ico`}/>
       <div className="flex-md-column m-1">
           <p>Сокращённая ссылка: <a href={shortRedirectionUrl}>{url.shortUrl}</a></p>
           <form className="m-1 flex-md-column" onSubmit={(e) => handleSubmit(e)}>
               <label className="m-1" htmlFor="edit-full-url">Полная ссылка:</label>
               <input className="m-1" name="newFullUrl" id="edit-full-url" type="url" placeholder="new url"
                      onChange={(e) => handleUrlChange(e.target.value)} 
                      value={formState.currentUrlValue}/>
               <input className="m-1" type="submit" value="Обновить" disabled={!formState.isChanged}/>
           </form>
       </div>
   </div>)
}