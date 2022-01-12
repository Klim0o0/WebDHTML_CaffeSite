import React, {useEffect, useState} from "react";
import {EmptyFunction, GetJson} from "../Utils/Utils";
import Loading from "./Loading";

export default function UserInfo() {
    const [userItem, setUserItem] = useState(null)
    const [bookingUserItem, setBookingUser] = useState(null)
    const [loaded, setLoaded] = useState(false)
    const [loaded2, setLoaded2] = useState(false)
    const [error, setError] = useState(null)
    useEffect(() => {
        GetJson("api/user/bookings", setBookingUser, setLoaded2, setError)
    }, [])
    useEffect(() => {
        GetJson("/api/Account/info", setUserItem, setLoaded, EmptyFunction)
    }, [])


    if (!userItem || !bookingUserItem) {
        return (<Loading/>)
    }

    if (error) {
        return (<h1>{error.message}</h1>)
    }
    return (<div>
        <p>
            Login: {userItem.Login}
        </p>
        {bookingUserItem.map(x => <div>Столик номер {x.Item1.TableNumber} Количество
            мест: {x.Item1.SeatsCount}
            <ul> {x.Item2.map(y => <li>c {y.DateFrom} по {y.DateTo} </li>)}</ul>
        </div>)}

    </div>)
}