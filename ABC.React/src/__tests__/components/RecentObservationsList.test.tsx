import React from "react"
import { render, screen } from "@testing-library/react"
import "@testing-library/jest-dom"
import { MemoryRouter } from "react-router-dom"
import { RecentObservationsList } from "../../components/RecentObservationsList"
import type { Observation } from "../../types/observation"

const mockObservations: Observation[] = [
  {
    id: "o1",
    notes: "First observation notes",
    status: "CLOSED",
    when: {
      startedAt: "2026-03-15T14:00:00Z",
      endedAt: "2026-03-15T15:00:00Z",
    },
    antecedents: [{ id: "1", name: "Loud noise" }],
    behaviors: [{ id: "1", name: "Hand flapping" }],
    consequences: [{ id: "1", name: "Timeout" }],
  },
  {
    id: "o2",
    notes: "",
    status: "OPEN",
    when: { startedAt: "2026-03-16T10:00:00Z", endedAt: null },
    antecedents: [],
    behaviors: [{ id: "2", name: "Screaming" }],
    consequences: [],
    child: { id: "child-1", firstName: "Jane", lastName: "Doe" },
  },
]

function renderWithRouter(ui: React.ReactElement) {
  return render(<MemoryRouter>{ui}</MemoryRouter>)
}

describe("RecentObservationsList", () => {
  it("renders the heading", () => {
    renderWithRouter(<RecentObservationsList observations={[]} />)
    expect(
      screen.getByText("Most Recent Observations")
    ).toBeInTheDocument()
  })

  it("shows empty message when no observations", () => {
    renderWithRouter(<RecentObservationsList observations={[]} />)
    expect(screen.getByText("No observations yet")).toBeInTheDocument()
  })

  it("renders observation notes", () => {
    renderWithRouter(<RecentObservationsList observations={mockObservations} />)
    expect(screen.getByText("First observation notes")).toBeInTheDocument()
  })

  it("renders status badges", () => {
    renderWithRouter(<RecentObservationsList observations={mockObservations} />)
    expect(screen.getByText("CLOSED")).toBeInTheDocument()
    expect(screen.getByText("OPEN")).toBeInTheDocument()
  })

  it("renders ABC tags", () => {
    renderWithRouter(<RecentObservationsList observations={mockObservations} />)
    expect(screen.getByText("A: Loud noise")).toBeInTheDocument()
    expect(screen.getByText("B: Hand flapping")).toBeInTheDocument()
    expect(screen.getByText("C: Timeout")).toBeInTheDocument()
    expect(screen.getByText("B: Screaming")).toBeInTheDocument()
  })

  it("shows Continue button for open observations with a child", () => {
    renderWithRouter(<RecentObservationsList observations={mockObservations} />)
    expect(screen.getByRole("button", { name: "Continue" })).toBeInTheDocument()
  })

  it("does not show Continue button for closed observations", () => {
    const closedOnly: Observation[] = [mockObservations[0]]
    renderWithRouter(<RecentObservationsList observations={closedOnly} />)
    expect(screen.queryByRole("button", { name: "Continue" })).not.toBeInTheDocument()
  })
})
