import "./App.css";
import { Route, Switch } from "wouter";
import { HealthcheckPage } from "./features/healthcheck/healthcheck-page.tsx";

function App() {
	return (
		<Switch>
			<Route path="/healthcheck" component={HealthcheckPage} />
			<Route>
				<p>Home</p>
			</Route>
		</Switch>
	);
}

export default App;
