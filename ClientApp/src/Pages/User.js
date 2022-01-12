import React, {useEffect, useState} from "react";
import LoginForm from "../Forms/LoginForm";
import UserInfo from "../Forms/UserInfo";
import RegistrationForm from "../Forms/RegistrationForm";

export default function User(props) {
    const [isReg, setIsReg] = useState(false)

    const a = event => {
        event.preventDefault();
        setIsReg(!isReg)
    }


    if (!props.IsA) {
        if (isReg) {
            return (<div><RegistrationForm setIsA={props.setIsA} setLoaded={props.setLoaded}/>
                <button onClick={a}>Назад</button>
            </div>);
        }

        return (<div><LoginForm setIsA={props.setIsA} setLoaded={props.setLoaded}/>
            <button onClick={a}>Зарегистрироватся</button>
        </div>);
    } else {
        return (<div><UserInfo/></div>
        );
    }
}