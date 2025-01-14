import React from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import styles from "./Login.module.css";

const schema = Yup.object().shape({
  username: Yup.string().required("Username é obrigatório"),
  password: Yup.string().required("Senha é obrigatória"),
});

export default function Login() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const navigate = useNavigate();

  const onSubmit = async (data) => {
    try {
      const response = await axios.post("http://localhost:5029/login", data);
      console.log(response.data);
      //sei que é nao era pra retornar o token dentro da response, 
      // mas nao estava conseguindo acessar o headers
      const token = response.data.token;
      if (token) {
        localStorage.setItem("token", token);
        localStorage.setItem("autenticado", true);
        navigate("/projetosGerais");
      } else {
        console.error("Token não encontrado na resposta");
      }
    } catch (error) {
      console.error("Erro no login", error);
    }
  };

  const handleRedirect = () => {
    navigate("/cadastro");
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className={styles.formContainer}>
      <div>
        <label htmlFor="username" className={styles.label}>
          Username
        </label>
        <input
          id="username"
          type="text"
          className={`${styles.input} ${
            errors.username ? styles.inputError : ""
          }`}
          {...register("username")}
        />
        {errors.username && (
          <p className={styles.errorMessage}>{errors.username.message}</p>
        )}
      </div>

      <div>
        <label htmlFor="password" className={styles.label}>
          Password
        </label>
        <input
          id="password"
          type="password"
          className={`${styles.input} ${
            errors.password ? styles.inputError : ""
          }`}
          {...register("password")}
        />
        {errors.password && (
          <p className={styles.errorMessage}>{errors.password.message}</p>
        )}
      </div>

      <button
        type="button"
        onClick={handleRedirect}
        className={styles.redirectButton}
      >
        <p>Ainda não possui uma conta?</p>
      </button>

      <button type="submit" className={styles.button}>
        Login
      </button>
    </form>
  );
}
