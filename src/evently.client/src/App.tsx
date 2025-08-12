import "./App.css";
import { Route, Switch } from "wouter";
import { HealthCheckPage } from "./features";

function App() {
	return (
		<Switch>
			<Route path="/healthcheck" component={HealthCheckPage} />
			<Route>
				<p>Home</p>
			</Route>
		</Switch>
	);
}

export default App;
