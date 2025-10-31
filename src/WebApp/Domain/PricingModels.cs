using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace WebApp.Domain;

/// <summary>
/// Represents a single feature within a pricing plan, including its content and availability state.
/// </summary>
public sealed record PricingFeature(MarkupString Content, bool IsAvailable);

/// <summary>
/// Data model for an individual pricing plan card.
/// </summary>
public sealed record PricingPlan(
    string Title,
    string Subtitle,
    string MonthlyPrice,
    string YearlyPrice,
    IReadOnlyList<PricingFeature> Features);

/// <summary>
/// Represents a comparison value within the comparison table.
/// </summary>
public sealed record PricingComparisonValue(bool? Included, string? Text);

/// <summary>
/// A row in the pricing comparison table describing availability per plan.
/// </summary>
public sealed record PricingComparisonRow(
    string Name,
    PricingComparisonValue Freelancer,
    PricingComparisonValue Company,
    PricingComparisonValue Enterprise);

/// <summary>
/// FAQ entry containing a question and its rich-text answer.
/// </summary>
public sealed record FaqItem(string Title, MarkupString Answer);

/// <summary>
/// Footer navigation link metadata.
/// </summary>
public sealed record FooterLink(string Item, string Href, string ClassName);

/// <summary>
/// Footer navigation section grouping related links.
/// </summary>
public sealed record FooterMenu(string Title, IReadOnlyList<FooterLink> Links);

/// <summary>
/// Footer branding data (logo and link).
/// </summary>
public sealed record FooterBrand(string Name, string Href, string Src, string Alt);

/// <summary>
/// Aggregated data required to render the pricing page.
/// </summary>
public sealed record PricingPageData(
    string Path,
    string Title,
    string Subtitle,
    string Description,
    string HeroTitle,
    string HeroLeadingText,
    IReadOnlyList<PricingPlan> Plans,
    IReadOnlyList<PricingComparisonRow> ComparisonRows,
    IReadOnlyList<FaqItem> Faqs,
    IReadOnlyList<FooterMenu> FooterMenus,
    FooterBrand Brand,
    MarkupString FooterDescription);
