import {useEffect, useState} from "react";
import {useParams} from "react-router-dom";

export default function ShortenedView(){
    const [data, setData] = useState({isLoading: true})
    const { link } = useParams();
    
    useEffect(() => {
        if (data.isLoading) {
            fetch(`/links/${link}`).then(res => {
                if (res.ok) res.json().then(data => setData({isLoading: false, url: data}))
            });
        }
    }, [data.isLoading])
    
    
}