import React, {useEffect, useState} from "react";
import {EmptyFunction, GetJson, Post, UrlWithQuery} from "../Utils/Utils";
import {queries, queryHelpers} from "@testing-library/react";

export default function Booking(props) {
    const [date, setDate] = useState(null)
    const [timeFrom, setTimeFrom] = useState(null)
    const [count, setCount] = useState(2)
    const [tables, setTables] = useState([])
    const [isLoaded, setIsLoaded] = useState(false)
    const [booked, setBooked] = useState(false)

    const bookedSwitch = () => {
        setBooked(!booked)
    }

    const getL = () => {
        if (date && timeFrom) {
            GetJson("/api/booking?from=" + date.toString() + "T" + timeFrom.toString() + ":00Z&count=" + count.toString(), setTables, setIsLoaded, EmptyFunction);
        }
    }

    useEffect(() => {
        getL()
    }, [booked])

    const book = (seatsCount) => {
        if (date && timeFrom) {
            Post("/api/booking?seatsCount=" + seatsCount.toString() + "&from=" + date.toString() + "T" + timeFrom.toString() + ":00Z&count=" + count.toString(), EmptyFunction, bookedSwitch, EmptyFunction);
        }
        getL()
    }


    return (<div>
        <form>
            <label>Дата</label>
            <input type="date" id="myDate" onChange={(x) => setDate(x.target.value)}/>
            <div>
                <label>Время</label>
                <input type="time" onChange={(x) => setTimeFrom(x.target.value)}/>
            </div>
            <div>
                <label>Количество часов</label>
                <input type="range" max={12} min={2} defaultValue={2} onChange={(x) => {
                    setCount(x.target.value);
                }}/>
                <label>{count}</label>
            </div>
            <button onClick={(x) => {
                x.preventDefault();
                getL()
            }}>Посмотреть свободные столики
            </button>
        </form>
        {isLoaded && tables.length > 0 ? <div>{tables.map(x => <div>
            <div>Столик номер:{x.TableNumber} Количество мест:{x.SeatsCount}</div>
            {props.IsA ?
                <button onClick={() => book(x.SeatsCount)}>Забронировать</button> : "Для бронирования необходимо войти"}
        </div>)}</div> : isLoaded ? "Нет свободных столиков на выбраное время" : ""}
    </div>)
}
