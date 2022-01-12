import React, {useEffect, useState} from "react";
import {EmptyFunction, Post} from "../Utils/Utils";
import Loading from "./Loading";

export default function LoginForm(props) {
    const [email, setEmail] = useState("");
    const [password, setPass] = useState("");
    const [loaded, setLoaded] = useState(false)
    const [started, setStarted] = useState(false)
    const [item, setItem] = useState(null);

    useEffect(x => {
        if (loaded) {
            setStarted(false)
        }
    }, [loaded])
    useEffect(x => {
        if (started) {
            setLoaded(false)
        }
    }, [started])

    useEffect(x => {
        if (item) {
            props.setIsA(item[0])
        }
    }, [item])


    const login = event => {
        event.preventDefault();
        Post('/api/Account/Login', setItem, setLoaded, EmptyFunction, JSON.stringify({
            Email: email, Password: password
        }))
        setStarted(true);
    }

    return (<form onSubmit={login}>
        <div><input type="text" name="email" value={email} placeholder="Email"
                    onChange={event => setEmail(event.target.value)}/></div>
        <div><input type="password" name="password" value={password}
                    placeholder="Пароль"
                    onChange={event => setPass(event.target.value)}/></div>
        <button>Войти</button>
        {started ? <Loading/> : <p1>{loaded ? item[1] : ""}</p1>}
    </form>)

}