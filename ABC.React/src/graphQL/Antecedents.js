import { gql } from '@apollo/client';

export const GET_ANTECEDENTS_QUERY = gql`
  query getAntecedents($count: Int!, $after: String) {
    antecedents(first: $count, after: $after, order: { id: DESC }) {      
      nodes {
        id
        name
        description
      }
      pageInfo {
        endCursor
        hasNextPage
        hasPreviousPage
        startCursor
      }
    }
  }
  `;