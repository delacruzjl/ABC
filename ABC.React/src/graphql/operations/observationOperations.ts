import { gql } from "@apollo/client"

export const DAILY_CONTEXT_FRAGMENT = gql`
  fragment DailyContextFields on DailyContext {
    hadBreakfast
    hadLunch
    hadDinner
    hadSnack
    sleptWell
    hoursOfSleep
  }
`

export const START_OBSERVATION = gql`
  mutation StartObservation($childId: UUID!, $dailyContext: DailyContextInput) {
    startObservation(childId: $childId, dailyContext: $dailyContext) {
      id
      notes
      status
      when {
        startedAt
        endedAt
      }
      dailyContext {
        ...DailyContextFields
      }
      antecedents {
        id
        name
      }
      behaviors {
        id
        name
      }
      consequences {
        id
        name
      }
      child {
        id
        firstName
        lastName
      }
    }
  }
  ${DAILY_CONTEXT_FRAGMENT}
`

export const UPDATE_OBSERVATION = gql`
  mutation UpdateObservation(
    $observationId: UUID!
    $antecedents: [UUID!]
    $behaviors: [UUID!]
    $consequences: [UUID!]
    $notes: String
    $dailyContext: DailyContextInput
  ) {
    updateObservation(
      command: {
        observationId: $observationId
        antecedents: $antecedents
        behaviors: $behaviors
        consequences: $consequences
        notes: $notes
        dailyContext: $dailyContext
      }
    ){
      id
      notes
      status
      when {
        startedAt
        endedAt
      }
      dailyContext {
        ...DailyContextFields
      }
      antecedents {
        id
        name
      }
      behaviors {
        id
        name
      }
      consequences {
        id
        name
      }
    }
  }
  ${DAILY_CONTEXT_FRAGMENT}
`

export const END_OBSERVATION = gql`
  mutation EndObservation($observationId: UUID!) {
    endObservation(observationId: $observationId) {
      id
      notes
      status
      when {
        startedAt
        endedAt
      }
      dailyContext {
        ...DailyContextFields
      }
      antecedents {
        id
        name
      }
      behaviors {
        id
        name
      }
      consequences {
        id
        name
      }
    }
  }
  ${DAILY_CONTEXT_FRAGMENT}
`

export const GET_OPEN_OBSERVATIONS = gql`
  query GetOpenObservations($childId: UUID!) {
    observations(
      where: { status: { eq: OPEN }, child: { id: { eq: $childId } } }
      first: 10
      order: [{ when: { startedAt: DESC } }]
    ) {
      nodes {
        id
        notes
        status
        when {
          startedAt
          endedAt
        }
        dailyContext {
          ...DailyContextFields
        }
        antecedents {
          id
          name
        }
        behaviors {
          id
          name
        }
        consequences {
          id
          name
        }
        child {
          id
          firstName
          lastName
        }
      }
    }
  }
  ${DAILY_CONTEXT_FRAGMENT}
`
