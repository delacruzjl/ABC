import { gql } from "@apollo/client"

export const LOGIN_MUTATION = gql`
  mutation Login($email: String!, $password: String!) {
    login(email: $email, password: $password) {
      token
      email
      roles
    }
  }
`

export const REGISTER_MUTATION = gql`
  mutation Register($email: String!, $password: String!) {
    register(email: $email, password: $password) {
      token
      email
      roles
    }
  }
`

export const ME_QUERY = gql`
  query Me {
    me {
      token
      email
      roles
    }
  }
`
