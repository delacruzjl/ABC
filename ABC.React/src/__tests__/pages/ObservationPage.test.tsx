import React from "react"
import { render, screen, waitFor, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import { ApolloClient, InMemoryCache } from "@apollo/client"
import { ApolloProvider } from "@apollo/client/react"
import { MockLink } from "@apollo/client/testing"
import { MemoryRouter, Route, Routes } from "react-router-dom"
import { ObservationPage } from "../../pages/ObservationPage"
import { GET_ANTECEDENTS } from "../../graphql/operations/antecedentOperations"
import { GET_BEHAVIORS } from "../../graphql/operations/behaviorOperations"
import { GET_CONSEQUENCES } from "../../graphql/operations/consequenceOperations"
import {
  START_OBSERVATION,
  UPDATE_OBSERVATION,
  END_OBSERVATION,
  GET_OPEN_OBSERVATIONS,
} from "../../graphql/operations/observationOperations"

interface MockDef {
  request: { query: any; variables?: any }
  result?: { data: any }
  error?: Error
}

const mockAntecedents = [
  { id: "a1", name: "Loud noise", description: "A sudden loud noise" },
  { id: "a2", name: "Denied request", description: "A request was denied" },
]

const mockBehaviors = [
  { id: "b1", name: "Crying", description: "Crying loudly" },
  { id: "b2", name: "Hitting", description: "Hitting others" },
]

const mockConsequences = [
  { id: "c1", name: "Redirected", description: "Redirected to activity" },
  { id: "c2", name: "Ignored", description: "Behavior was ignored" },
]

const childId = "child-123"

const listMocks: MockDef[] = [
  {
    request: { query: GET_ANTECEDENTS },
    result: { data: { antecedents: { nodes: mockAntecedents } } },
  },
  {
    request: { query: GET_BEHAVIORS },
    result: { data: { behaviors: { nodes: mockBehaviors } } },
  },
  {
    request: { query: GET_CONSEQUENCES },
    result: { data: { consequences: { nodes: mockConsequences } } },
  },
  {
    request: { query: GET_OPEN_OBSERVATIONS, variables: { childId } },
    result: { data: { observations: { nodes: [] } } },
  },
]

const mockObservation = {
  id: "obs-1",
  notes: "",
  status: "OPEN",
  when: { startedAt: "2025-01-01T10:00:00Z", endedAt: null },
  dailyContext: null,
  antecedents: [],
  behaviors: [],
  consequences: [],
  child: { id: childId, firstName: "Jane", lastName: "Doe" },
}

const defaultDailyContext = {
  hadBreakfast: false,
  hadLunch: false,
  hadDinner: false,
  hadSnack: false,
  sleptWell: false,
  hoursOfSleep: null,
}

function renderPage(extraMocks: MockDef[] = []) {
  const mocks = [...listMocks, ...extraMocks]
  const link = new MockLink(mocks, { addTypename: false })
  const client = new ApolloClient({
    link,
    cache: new InMemoryCache({ addTypenames: false }),
  })

  return render(
    <ApolloProvider client={client}>
      <MemoryRouter initialEntries={[`/observation/${childId}`]}>
        <Routes>
          <Route path="/observation/:childId" element={<ObservationPage />} />
          <Route path="/children" element={<div>Children Page</div>} />
        </Routes>
      </MemoryRouter>
    </ApolloProvider>
  )
}

describe("ObservationPage", () => {
  it("shows loading state while fetching lists", async () => {
    renderPage()
    expect(screen.getByText(/Loading observation data/)).toBeInTheDocument()
    // Wait for pending effects to avoid interference with subsequent tests
    await waitFor(() => {
      expect(screen.queryByText(/Loading observation data/)).not.toBeInTheDocument()
    })
  })

  it("shows pre-start state after lists load", async () => {
    renderPage()

    await waitFor(() => {
      expect(
        screen.getByRole("button", { name: "Start New Observation" })
      ).toBeInTheDocument()
    })

    expect(
      screen.getByText(/Begin an ABC data collection session/)
    ).toBeInTheDocument()

    // Wait for any pending lazy query effects to settle
    await waitFor(() => {})
  })

  it("starts observation and shows active recording view", async () => {
    const startMock: MockDef = {
      request: {
        query: START_OBSERVATION,
        variables: { childId, dailyContext: defaultDailyContext },
      },
      result: { data: { startObservation: mockObservation } },
    }

    renderPage([startMock])

    await waitFor(() => {
      expect(
        screen.getByRole("button", { name: "Start New Observation" })
      ).toBeInTheDocument()
    })

    fireEvent.click(screen.getByRole("button", { name: "Start New Observation" }))

    await waitFor(() => {
      expect(screen.getByText("Recording Observation")).toBeInTheDocument()
    })

    expect(screen.getByText("Jane Doe", { exact: false })).toBeInTheDocument()
    expect(screen.getByText("Loud noise")).toBeInTheDocument()
    expect(screen.getByText("Crying")).toBeInTheDocument()
    expect(screen.getByText("Redirected")).toBeInTheDocument()
  })

  it("prevents ending observation without selections", async () => {
    const startMock: MockDef = {
      request: {
        query: START_OBSERVATION,
        variables: { childId, dailyContext: defaultDailyContext },
      },
      result: { data: { startObservation: mockObservation } },
    }

    renderPage([startMock])

    await waitFor(() => {
      expect(
        screen.getByRole("button", { name: "Start New Observation" })
      ).toBeInTheDocument()
    })

    fireEvent.click(screen.getByRole("button", { name: "Start New Observation" }))

    await waitFor(() => {
      expect(screen.getByText("Recording Observation")).toBeInTheDocument()
    })

    const endButton = screen.getByRole("button", { name: "End Observation" })
    expect(endButton).toBeDisabled()

    expect(
      screen.getByText(/Select at least one antecedent/)
    ).toBeInTheDocument()
  })

  it("enables end button after selecting one of each ABC category", async () => {
    const startMock: MockDef = {
      request: {
        query: START_OBSERVATION,
        variables: { childId, dailyContext: defaultDailyContext },
      },
      result: { data: { startObservation: mockObservation } },
    }

    renderPage([startMock])

    await waitFor(() => {
      expect(
        screen.getByRole("button", { name: "Start New Observation" })
      ).toBeInTheDocument()
    })

    fireEvent.click(screen.getByRole("button", { name: "Start New Observation" }))

    await waitFor(() => {
      expect(screen.getByText("Recording Observation")).toBeInTheDocument()
    })

    // Select one antecedent, one behavior, one consequence
    fireEvent.click(screen.getByText("Loud noise"))
    fireEvent.click(screen.getByText("Crying"))
    fireEvent.click(screen.getByText("Redirected"))

    const endButton = screen.getByRole("button", { name: "End Observation" })
    expect(endButton).not.toBeDisabled()

    expect(
      screen.queryByText(/Select at least one antecedent/)
    ).not.toBeInTheDocument()
  })

  it("completes full observation lifecycle", async () => {
    const startMock: MockDef = {
      request: {
        query: START_OBSERVATION,
        variables: { childId, dailyContext: defaultDailyContext },
      },
      result: { data: { startObservation: mockObservation } },
    }

    const updatedObs = {
      ...mockObservation,
      notes: "Test note",
      antecedents: [{ id: "a1", name: "Loud noise" }],
      behaviors: [{ id: "b1", name: "Crying" }],
      consequences: [{ id: "c1", name: "Redirected" }],
    }

    const updateMock: MockDef = {
      request: {
        query: UPDATE_OBSERVATION,
        variables: {
          observationId: "obs-1",
          antecedents: ["a1"],
          behaviors: ["b1"],
          consequences: ["c1"],
          notes: "Test note",
          dailyContext: defaultDailyContext,
        },
      },
      result: { data: { updateObservation: updatedObs } },
    }

    const endedObs = {
      ...updatedObs,
      status: "CLOSED",
      when: {
        startedAt: "2025-01-01T10:00:00Z",
        endedAt: "2025-01-01T10:15:00Z",
      },
    }

    const endMock: MockDef = {
      request: {
        query: END_OBSERVATION,
        variables: { observationId: "obs-1" },
      },
      result: { data: { endObservation: endedObs } },
    }

    renderPage([startMock, updateMock, endMock])

    // Wait for lists, then start
    await waitFor(() => {
      expect(
        screen.getByRole("button", { name: "Start New Observation" })
      ).toBeInTheDocument()
    })
    fireEvent.click(screen.getByRole("button", { name: "Start New Observation" }))

    await waitFor(() => {
      expect(screen.getByText("Recording Observation")).toBeInTheDocument()
    })

    // Select items
    fireEvent.click(screen.getByText("Loud noise"))
    fireEvent.click(screen.getByText("Crying"))
    fireEvent.click(screen.getByText("Redirected"))

    // Add notes
    const notesTextarea = screen.getByPlaceholderText(
      /Add any additional context/
    )
    fireEvent.change(notesTextarea, { target: { value: "Test note" } })

    // End observation
    fireEvent.click(screen.getByRole("button", { name: "End Observation" }))

    await waitFor(() => {
      expect(screen.getByText("Observation Complete")).toBeInTheDocument()
    })
  })

  it("shows error when start observation fails", async () => {
    const startMock: MockDef = {
      request: {
        query: START_OBSERVATION,
        variables: { childId, dailyContext: defaultDailyContext },
      },
      error: new Error("Server error"),
    }

    renderPage([startMock])

    await waitFor(() => {
      expect(
        screen.getByRole("button", { name: "Start New Observation" })
      ).toBeInTheDocument()
    })

    fireEvent.click(screen.getByRole("button", { name: "Start New Observation" }))

    await waitFor(() => {
      expect(screen.getByText("Server error")).toBeInTheDocument()
    })
  })

  it("shows empty list message when no items available", async () => {
    const emptyListMocks: MockDef[] = [
      {
        request: { query: GET_ANTECEDENTS },
        result: { data: { antecedents: { nodes: [] } } },
      },
      {
        request: { query: GET_BEHAVIORS },
        result: { data: { behaviors: { nodes: [] } } },
      },
      {
        request: { query: GET_CONSEQUENCES },
        result: { data: { consequences: { nodes: [] } } },
      },
      {
        request: { query: GET_OPEN_OBSERVATIONS, variables: { childId } },
        result: { data: { observations: { nodes: [] } } },
      },
    ]

    const startMock: MockDef = {
      request: {
        query: START_OBSERVATION,
        variables: { childId, dailyContext: defaultDailyContext },
      },
      result: { data: { startObservation: mockObservation } },
    }

    const link = new MockLink([...emptyListMocks, startMock], {
      addTypename: false,
    })
    const client = new ApolloClient({
      link,
      cache: new InMemoryCache({ addTypenames: false }),
    })

    render(
      <ApolloProvider client={client}>
        <MemoryRouter initialEntries={[`/observation/${childId}`]}>
          <Routes>
            <Route
              path="/observation/:childId"
              element={<ObservationPage />}
            />
          </Routes>
        </MemoryRouter>
      </ApolloProvider>
    )

    await waitFor(() => {
      expect(
        screen.getByRole("button", { name: "Start New Observation" })
      ).toBeInTheDocument()
    })

    fireEvent.click(screen.getByRole("button", { name: "Start New Observation" }))

    await waitFor(() => {
      expect(screen.getByText("Recording Observation")).toBeInTheDocument()
    })

    expect(
      screen.getByText(/No antecedents available/)
    ).toBeInTheDocument()
    expect(
      screen.getByText(/No behaviors available/)
    ).toBeInTheDocument()
    expect(
      screen.getByText(/No consequences available/)
    ).toBeInTheDocument()
  })
})
