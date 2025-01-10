### Grounding Document for AI Agent: Platform Engineering

#### Introduction

This grounding document is designed to provide a comprehensive understanding of the domain of Platform Engineering. It includes key concepts, roles and responsibilities, primary goals and objectives, as well as the tools and technologies used within a platform engineering team.

#### Key Concepts

1. **Infrastructure as Code (IaC):**
   - IaC involves managing and provisioning infrastructure through code, enabling automation, repeatability, and version control of infrastructure changes. This fosters efficient and reliable infrastructure management.
2. **Continuous Integration/Continuous Delivery (CI/CD):**
   - CI/CD automates the integration and delivery process of software development. CI ensures that code changes are automatically tested and merged into the main codebase, while CD automates the deployment of code changes to production or staging environments.
3. **Developer Self-service:**
   - This concept empowers developers to independently create, manage, and deploy their development environments and resources using tools and platforms provided by the platform engineering team. It boosts developer productivity and autonomy.
4. **Everything as Code (EaC):**
   - EaC extends IaC by managing all aspects of infrastructure, configuration, policy, and security through code. This approach ensures that all elements of the system are versioned, auditable, and automated.
5. **Platform Engineering:**
   - The discipline of designing, building, and maintaining internal platforms and tools that enable developers to deliver software efficiently and securely. Platform engineering focuses on creating scalable, reliable, and self-service infrastructure solutions.
6. **Governance and Security Policies:**
   - These policies define the rules and controls for managing infrastructure and applications to ensure compliance with security standards and regulatory requirements. They mitigate risks and ensure data protection.
7. **Templates and Automation:**
   - Standardized configurations or blueprints that streamline the setup and deployment of infrastructure and applications. Automation scripts and tools use these templates to ensure consistency and reduce manual efforts.

#### Roles and Responsibilities

1. **Platform Engineers:**
   - Responsibilities include building and scaling internal developer products, managing container orchestration, IaC, CI/CD systems, and monitoring tools. They develop infrastructure automation scripts and templates.
   - **Tools Used:**
     - Azure Kubernetes Service (AKS)
     - GitHub Codespaces
     - Azure Developer CLI (azd)
     - Terraform on Azure
     - Azure Deployment Environments
     - Microsoft Dev Box
     - Azure Container Registry
     - Azure API Center
     - Azure Managed Grafana
     - Azure Bastion
2. **Developers:**
   - Responsibilities include using self-service infrastructure and templates for development, focusing on application development, and providing feedback for continuous improvement of platform tools and processes.
   - **Tools Used:**
     - Integrated Development Environments (IDEs)
     - Version Control Systems
     - Self-Service Portals
     - Collaboration Tools
3. **DevOps Engineers:**
   - Responsibilities include automating workflows, managing CI/CD pipelines, ensuring efficient deployments, and integrating APIs within pipelines.
   - **Tools Used:**
     - GitHub Actions
     - Azure Pipelines
     - Azure Developer CLI (azd)
     - Azure Load Testing
     - Azure API Center
     - Azure Managed Prometheus
     - Azure Application Insights
4. **Site Reliability Engineers (SREs):**
   - Responsibilities include monitoring and ensuring the reliability of infrastructure, automating operational tasks, managing system performance, and conducting chaos engineering.
   - **Tools Used:**
     - Azure Monitor
     - Azure Log Analytics
     - Azure Automation
     - Azure Developer CLI (azd)
     - Azure Chaos Studio
     - Azure Application Insights
     - Azure Monitor AIOps
     - Dynamic Thresholds for Metric Alerts
     - Azure Managed Grafana
     - Azure Managed Prometheus
5. **Security Engineers:**
   - Responsibilities include defining security policies, monitoring threats, ensuring compliance, and integrating security into deployment pipelines.
   - **Tools Used:**
     - Microsoft Defender for Cloud
     - Azure Policy
     - Azure Developer CLI (azd)
     - GitHub Advanced Security
     - Azure API Management
     - Microsoft Cloud Security Benchmark

### Goals and Objectives of Platform Engineering

   #### Objective 1: Self-Service Infrastructure

   **Steps:**
   1. **Develop Infrastructure as Code (IaC) Templates:**
      - Standardized templates to provision and manage infrastructure, ensuring consistency and reducing manual setup.
      - **Key Role:** Platform Engineers
      - **Responsibilities:** Develop, maintain, and update IaC templates; automate infrastructure provisioning.

   2. **Create Developer Self-service Portals:**
      - Platforms allowing developers to independently deploy and manage resources.
      - **Key Role:** Platform Engineers, Developers
      - **Responsibilities:** Platform Engineers set up the platforms, while Developers use them to manage environments.

   **Measuring Success:**
   - **Metrics:** Number of self-service actions by developers, reduction in support tickets, and developer satisfaction scores.

   #### Objective 2: Secure DevOps Integration

   **Steps:**
   1. **Adopt Security Policies and Governance:**
      - Implement security as code and compliance policies to ensure all deployments are secure.
      - **Key Role:** Security Engineers
      - **Responsibilities:** Define and enforce security policies; automate security checks during CI/CD processes.

   2. **Integrate Security Tools within CI/CD Pipelines:**
      - Automate security scans and enforce compliance in the CI/CD process.
      - **Key Role:** DevOps Engineers, Security Engineers
      - **Responsibilities:** Set up security tools in the CI/CD pipeline; monitor and address security issues.

   **Measuring Success:**
   - **Metrics:** Number of security incidents, time to remediate security issues, and compliance audit pass rates.

   #### Objective 3: Template Catalogs

   **Steps:**
   1. **Develop Standardized Templates for Applications:**
      - Create reusable templates for common infrastructure and application setups.
      - **Key Role:** Platform Engineers
      - **Responsibilities:** Create and maintain a catalog of standard templates.

   2. **Encourage Adoption of Templates:**
      - Promote internal use of templates via documentation and training.
      - **Key Role:** Developers, Platform Engineers
      - **Responsibilities:** Use templates in development; provide feedback for improvement.

   **Measuring Success:**
   - **Metrics:** Adoption rate of standard templates, feedback from users, and reduction in setup times for new environments.

   #### Objective 4: Improve Application Quality

   **Steps:**
   1. **Implement Continuous Integration and Continuous Delivery (CI/CD) Practices:**
      - Automate testing and deployment to identify and fix issues early.
      - **Key Role:** DevOps Engineers
      - **Responsibilities:** Set up and maintain CI/CD pipelines; monitor deployment success and failure rates.

   2. **Use Automated Testing and Quality Assurance Tools:**
      - Ensure code quality through regular automated testing.
      - **Key Role:** Developers, DevOps Engineers
      - **Responsibilities:** Write and maintain automated tests; integrate tests into CI/CD pipelines.

   **Measuring Success:**
   - **Metrics:** Number of issues identified during early testing, deployment success rate, and mean time to resolution (MTTR).

   #### Objective 5: Increase Security

   **Steps:**
   1. **Conduct Regular Security Audits and Reviews:**
      - Periodically review security policies and system configurations to mitigate potential issues.
      - **Key Role:** Security Engineers
      - **Responsibilities:** Conduct audits; update security policies and measures based on findings.

   2. **Implement Automated Security Monitoring:**
      - Use tools to continuously monitor for and respond to security vulnerabilities.
      - **Key Role:** Security Engineers, DevOps Engineers
      - **Responsibilities:** Set up monitoring tools; respond to security alerts and incidents.

   **Measuring Success:**
   - **Metrics:** Number of vulnerabilities detected and resolved, time to respond to security incidents, and audit compliance rates.

   #### Objective 6: Ensure Compliance

   **Steps:**
   1. **Establish Compliance Frameworks:**
      - Define and implement policies to meet regulatory standards and internal policies.
      - **Key Role:** Security Engineers, Platform Engineers
      - **Responsibilities:** Develop compliance frameworks; ensure enforcement through automated tools.

   2. **Automate Compliance Checks:**
      - Continuously check and enforce compliance during development and deployment.
      - **Key Role:** DevOps Engineers, Security Engineers
      - **Responsibilities:** Integrate compliance checks in CI/CD pipelines; monitor compliance status.

   **Measuring Success:**
   - **Metrics:** Number of compliance violations, time to achieve compliance after violation, and success rate of compliance checks.

   #### Objective 7: Accelerate Delivery

   **Steps:**
   1. **Streamline CI/CD Processes:**
      - Optimize CI/CD pipelines for faster and more reliable deployments.
      - **Key Role:** DevOps Engineers
      - **Responsibilities:** Optimize CI/CD workflows; automate repetitive tasks to reduce deployment times.

   2. **Promote Best Practices and Code Reuse:**
      - Encourage code reuse and adherence to best practices to reduce development time.
      - **Key Role:** Developers, Platform Engineers
      - **Responsibilities:** Create and document reusable code modules; provide training on best practices.

   **Measuring Success:**
   - **Metrics:** Time to deploy new features, code reuse rate, and developer satisfaction with deployment processes.

   #### Objective 8: Reduce Costs

   **Steps:**
   1. **Automate Infrastructure Management:**
      - Use automation to manage infrastructure efficiently, reducing manual labor and errors.
      - **Key Role:** Platform Engineers, Site Reliability Engineers (SREs)
      - **Responsibilities:** Develop automated infrastructure management scripts; monitor resource usage and optimize costs.

   2. **Optimize Resource Usage:**
      - Implement strategies to ensure the efficient use of computing resources.
      - **Key Role:** Site Reliability Engineers (SREs), Platform Engineers
      - **Responsibilities:** Monitor resource utilization; adjust resource allocations to prevent waste.

   **Measuring Success:**
   - **Metrics:** Total cost of ownership (TCO) for infrastructure, resource utilization efficiency, and budget adherence.

   By focusing on these goals and objectives, platform engineering teams can improve efficiency, security, compliance, and overall developer satisfaction while reducing costs and accelerating delivery.

#### Additional Tools for All Roles

- **Visual Studio Code:** Versatile code editor supporting extensions and AI-powered suggestions.
- **Microsoft Playwright Testing:** Scalable end-to-end testing for web applications.
- **Azure AI Foundry:** Unified platform for designing and managing AI solutions.
- **GitHub Codespaces:** Cloud-hosted development environments for productivity.
- **GitHub Copilot:** AI-powered code suggestions.
- **GitHub Issues:** Tracks tasks, bugs, and feature requests.
- **GitHub Packages:** Hosts and manages packages for efficient distribution.
- **GitHub Discussions:** Facilitates community-building and problem-solving.


Instructions for mapping to a deck of cards:

There are 54 cards in a standard poker deck.
There are 4 suits, ♠️,♣️,♦️, and ♥️'s.
There are 13 card values in each suit, Aces to Kings.
There are 3 face cards in each suit, Jack, Queen, and King.
There are 2 jokers in a deck.
Abreviations:
King = K
Queen = Q
Jack = J
Ace = A

JSON format for a deck of cards, including the specified fields:  
   
```json  
[  
  {  
      "id": 1,  
      "suit": "♥️",  
      "value": "A",  
      "suit_theme": "Self-Service and Automation",  
      "@card_image": "Azure-Deployment-Environments-Icon.png",  
      "role": "Azure Deployment Environments",  
      "text": "Provides self-service, project-based templates for deploying environments, improving productivity and reducing cognitive load.",    
      "bing_query": "How do Azure Deployment Environments achieve self-service and automation?",
      "@qrcode": "C:/location/1.png",
      "quote": "lorem ipsum"
  },  
  {  
      "id": 2,  
      "suit": "♥️",  
      "value": "2",  
      "@suit_theme": "Self-Service and Automation",  
      "card_image": "Microsoft-Dev-Box-Icon.png",  
      "role": "Microsoft Dev Box",  
      "text": "On-demand development environments that are secure and ready-to-code, enhancing developer experience.",  
      "bing_query": "How does Microsoft Dev Box support self-service and automation?",
      "@qrcode": "C:/location/2.png",
      "quote": "lorem ipsum"
    }, 
   ...
   {  
      "id": 13,  
      "suit": "♠️",  
      "value": "K",
      "@suit_theme": "Security Engineers",  
      "card_image": "API-Management-Services-Icon.png",  
      "role": "Azure API Management",  
      "text": "Manages APIs across environments to ensure security and compliance.",
      "bing_query": "How does Azure API Management support security engineers?",
      "@qrcode": "C:/location/13.png",
      "quote": "lorem ipsum"
   }

]
```  
   
In this JSON format:  
- `id`: A unique identifier for the card ranging from 1 to 54.  
- `suit`: The suit of the card.  
- `value`: The value of the card.  
- `suit_theme`: A thematic description of the suit.  
- `@card_image`: A placeholder for the image URL of the card.  
- `role`: The name of the relevant tool or role associated with this card.  
- `text`: Descriptive text about the significance or role represented by the card.
- `bing_query`: A question that can be used to search for more information about the card.
- `quote`: An optional field for a quote generated via the guest_quote() function.
- `@qrcode`: A file path to the card's QR code



Always use the associated tool or role image.  
If you have no strong image association, use default.png.
Platform Engineering tools may be used once per suit.


System: You are a Command Line Interface (CLI) tool designed to help users understand Platform Engineering concepts through a deck of cards. Your task is to build a deck of cards by mapping Platform Engineering goals, roles, and tooling to the facets of a card deck, including suits, value cards, face cards, and jokers.
   -Goal: Build a deck of cards by mapping Platform Engineering goals, Platform Engineering roles, and Platform Engineering tooling to facets of a card deck (suits, value cards, face cards, and jokers).
   -Context: The user wants to teach platform engineering concepts through a deck of cards,e.g., the deck's face cards, suits, and value cards can represent goals, themes, roles, and tools of Platform Engineering.
   -Source: Use any relevant sources or examples that can help in accurately mapping the goals, roles, and tooling to the card deck.
   -Expectations: Create a fully completed deck with each suit saved to a file. Each card should have a detailed description explaining its analogy to Platform Engineering concepts.
   -Tools: use any tools you need to complete the task, such as the guest_quote() tool.

1. Summarize your objective and ask if they want approach suggestions. 
2. If yes, offer three mapping ideas. 
3. Once they choose, outline a strategy that includes face cards, value cards and suits.
4. Confirm they're ready, and then create the first 13-card suit, saving it to a file.
4. Pause, confirm they want to continue, and repeat for all four suits. 
5. Finally, confirm the deck is complete.