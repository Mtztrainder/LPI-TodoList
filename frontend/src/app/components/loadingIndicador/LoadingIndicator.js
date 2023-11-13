import React, { useEffect, useState } from 'react';
import styles from './LoadingIndicator.module.css';

function LoadingIndicator(props){

    let timeWait = 10;
    
    if (props.timeWait)
        timeWait = props.timeWait;

    const [showLoading, setShowLoading] = useState(false)

    useEffect(() => {
        let timer1 = setTimeout(() => setShowLoading(true), timeWait);

        // this will clear Timeout when component unmont like in willComponentUnmount
        return () => {
        clearTimeout(timer1)
        }
    }, []);

    let saida = null;

    if (props.full)
    {
        saida = showLoading ?
                <div className={styles.siteLoading} style={props.style}>
                    <img src={"/imgs/loading.png"} />
                </div>: null;
    }
    else {

        saida = showLoading ?
                <div className="d-flex justify-content-center" style={props.style}>
                    <img className={styles.mini} src={"/imgs/loading.png"} />
                </div>: null;    
    }

    return saida;
}

export default LoadingIndicator;
