import {useEffect, useState} from "react";
import {EmptyFunction, Get} from "../Utils/Utils";
import {Link} from "react-router-dom";

export default function LoginLogout(props) {
    useEffect(() => {
        Get("/api/Account/IsAuth", props.setIsA, props.setLoaded)
    })
    if (props.IsA) {
        return (<a onClick={() => {
            Get("/api/Account/Logout", EmptyFunction, EmptyFunction)
            props.setIsA(false)
        }
        }>
            Выйти
        </a>)
    }

    return (<Link to="/user">Войти</Link>)
}