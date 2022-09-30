import React,{useState,useContext} from "react";
import AuthContext from "../context/AuthContext";
import { Navigate } from "react-router-dom";
import { Col, Button, Row, Container, Card, Form } from "react-bootstrap";

const LoginV2 = () => {
  const {loginUser,user} = useContext(AuthContext);
    const [username,setUsername] = useState('');
    const [password,setPassword] = useState('');

    const validateForm = () => {
        return username.length > 0 && password.length > 0;
    }

    if(user){
        return <Navigate to="/"></Navigate>
    }

  return (
    <div>
      <Container>
        <Row className="vh-100 d-flex justify-content-center align-items-center">
          <Col md={8} lg={6} xs={12}>
            <div className="border border-3 border-primary"></div>
            <Card className="shadow">
              <Card.Body>
                <div className="mb-3 mt-md-4">
                  <h2 className="fw-bold mb-2 text-uppercase ">Login</h2>
                  <div className="mb-3">
                    <Form onSubmit={loginUser}>
                      <Form.Group className="mb-3" controlId="formBasicEmail">
                        <Form.Label className="text-center">
                          Username
                        </Form.Label>
                        <Form.Control name="username" type="username" placeholder="Enter username" value={username}
                                      onChange={(e) => setUsername(e.target.value)} />
                      </Form.Group>

                      <Form.Group
                        className="mb-3"
                        controlId="formBasicPassword"
                      >
                        <Form.Label>Password</Form.Label>
                        <Form.Control name="password" type="password" placeholder="Password" value={password}
                                      onChange={(e) => setPassword(e.target.value)}/>
                      </Form.Group>
                      <Form.Group
                        className="mb-3"
                        controlId="formBasicCheckbox"
                      >
                      </Form.Group>
                      <div className="d-grid">
                      <Button block="true" size="lg" type="submit" disabled={!validateForm()}>
                      Login
                      </Button>
                      </div>
                    </Form>
                    <div className="mt-3">
                      <p className="mb-0  text-center">
                        <a href="{''}" className="text-primary fw-bold">
                          Sign Up
                        </a>
                      </p>
                    </div>
                  </div>
                </div>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </Container>
    </div>
  );
}

export default LoginV2;