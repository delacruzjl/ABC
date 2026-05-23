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

export const EXTERNAL_LOGIN_MUTATION = gql`
  mutation ExternalLogin($provider: ExternalAuthProvider!, $idToken: String!) {
    externalLogin(provider: $provider, idToken: $idToken) {
      token
      email
      roles
    }
  }
`

export const DEV_EXTERNAL_LOGIN_MUTATION = gql`
  mutation DevExternalLogin($provider: ExternalAuthProvider!, $email: String!) {
    devExternalLogin(provider: $provider, email: $email) {
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
