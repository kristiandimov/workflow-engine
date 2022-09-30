import Form from 'react-bootstrap/Form';
import { Button } from "react-bootstrap";
import React,{useEffect, useState,useContext} from "react";
import AuthContext from "../../context/AuthContext";
import useAxios from "../../utils/useAxios";


const Flow = () => {
    let {user,tokens} = useContext(AuthContext)
    let [flowConfig,setFlowConfig] = useState("");
    let [flowResult,setFlowResult] = useState("");
    let [editable,setEditable] = useState(true);
    let api = useAxios();

    const disable = () =>
    {
        setEditable(!editable);
    }

    const getConfig = async () => {
        let response = await api.get("/api/Flow?id="+ Number(user.LoggedUserId))
        .then(response => {
            let data = response.data.config;
            setFlowConfig(data);
        })
    }

    const execute = async () => {
        const url = 'http://localhost:10741/api/Flow?id='+ Number(user.LoggedUserId);
        let response = await fetch(url, {
            method:'EXECUTE',
            headers:{
                'Content-Type':'application/json',
                'Authorization': 'Bearer ' + tokens.accessToken,
            },

        })
        let data = await response.json();

        setFlowResult(String(data.result));
    };

    //CreateUsers
    const saveConfig = async () =>{
        const flow = {
            'id':Number(user.LoggedUserId),
            'FlowConfig':flowConfig
        }

        let response = await api.post("/api/Flow",flow);
        disable();
    }
    
    useEffect(() => {
        getConfig();   
           
    },[]);

  return (
    <div className="container">
        <h1>Flow configuration</h1>
    <Form>
        <Button 
            className="btn-group me-1 float-right"
            variant="danger"
            onClick={() => {execute()}} 
            >Execute</Button>
        <Form.Group className="mb-3" controlId="exampleForm.ControlTextarea1">
            <Form.Label>Put your JSON configuration here</Form.Label>
            <Form.Control 
                as="textarea" 
                rows={10} 
                defaultValue = {flowConfig ? JSON.stringify(JSON.parse(flowConfig)) : null}
                onChange={(e) => setFlowConfig(e.target.value)} 
                disabled = {editable}/>
        </Form.Group>
        <Button 
            className="btn-group me-1" 
            variant="warning"
            onClick={() => {
                disable();
            }}
            >Edit Config</Button>
        <Button 
            className="btn-group me-1"
            onClick={() => {
                saveConfig()}}>Save Config</Button>
        <Form.Group className="mt-5" controlId="exampleForm.ControlTextarea1">
            <Form.Label>Output preview</Form.Label>
            <Form.Control 
                as="textarea" 
                rows={10} 
                defaultValue = {flowResult ? flowResult : null}
                disabled/>
        </Form.Group>
    </Form>
    </div>
  );
}

export default Flow;