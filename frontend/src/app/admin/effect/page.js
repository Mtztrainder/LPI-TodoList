"use client"

import { useEffect, useState } from "react"
  
 export default function Effect() {

    const [count, setCount] = useState(0);

    useEffect(() => {
        //Ocorre quando necessário causar um efeito após o RENDER
        //devido a alteração de qualquer state
        console.log("sempre que ocorrer render");

        //Exemplo: Monitorar alterações no DOM para aplicar ações externas
        //pode resultar em comportamentos indesejados se não for gerenciado corretamente
    });

    useEffect(() => {
        //Ocorre quando necessário causar um efeito após o RENDER
        //devido a alteração de um determinado state
        console.log("Sempre que count (state) for alterado");
        document.title = `Conta: ${count}`;

        //Exemplo: enviar para o backend o contador (carrinho de compra, quantidade selecionada)...
        // if (count < 30) // tomar cuidado, pois pode dar looping infinito 
        //     contar()
    }, [count]);

    useEffect(() => {
        //Ocorre quando o componente é montado (primeiro RENDER).
        console.log("Apenas após a montagem do componente (primeiro render)");
    
        //Exemplo: Ideal para obter dados na API

    }, []);


    const contar = () => {
        setCount(count + 1);
    }


    return (
        <div>
            <button className="btn btn-outline-secondary" 
                    type="button"
                    onClick={contar}>Contar</button>
        </div>
    );
}