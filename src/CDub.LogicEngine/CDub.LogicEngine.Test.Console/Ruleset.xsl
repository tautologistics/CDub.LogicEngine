<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">

	<xsl:template match="/ruleset">
		<html>
			<head>
				<title>Ruleset</title>
				<style type="text/css">
					body {
						font-family: courier;
					}
					
					h1 {
						font-family: arial;
					}

					ul, li {
						padding: 0px;
						margin: 0px;
						margin-left: 2em;
					}

					.operator {
						display: inline;
						padding-left: 0.2em;
						padding-right: 0.2em;
					}

					.operand {
						display: inline;
					}

					.keyword {
						display: inline;
						color: #00f;
						padding-left: 0.2em;
						padding-right: 0.2em;
					}

					.variable {
						display: inline;
						color: #000;
					}

					.text {
						display: inline;
						color: #d00;
					}

				</style>
			</head>
			<body><xsl:apply-templates select="./rule" mode="toplevel" /></body>
		</html>
	</xsl:template>

	<xsl:template match="rule" mode="toplevel">
		<h1><xsl:value-of select="@id" /></h1>
		
		<xsl:apply-templates select="." />
	</xsl:template>

	<xsl:template match="rule">
		<div class="keyword">if</div>
		<xsl:if test="not(./conditions)">(<div class="keyword">true</div>)</xsl:if>
		<xsl:apply-templates select="./conditions"/>
		<xsl:text>{</xsl:text><br/>
		<xsl:apply-templates select="./success"/>
		<xsl:if test="./failure">
			<xsl:text>}</xsl:text><br/>
			<div class="keyword">else</div><xsl:text>{</xsl:text><br/>
			<xsl:apply-templates select="./failure"/>
		</xsl:if>
		<xsl:text>}</xsl:text><br/>
	</xsl:template>

	<xsl:template match="success|failure">
		<ul>
			<xsl:apply-templates select="./output|./rule"/>
		</ul>
	</xsl:template>

	<xsl:template match="output">
		<div class="variable">output</div><xsl:text>[</xsl:text>
		<div class="text">'<xsl:value-of select="key"/>'</div>
		<xsl:text>] = </xsl:text>
		<xsl:apply-templates select="value"/>
		<xsl:text>;</xsl:text>
		<br/>
	</xsl:template>

	<xsl:template match="conditions">
		<xsl:variable name="op">
			<xsl:choose>
				<xsl:when test="./@op = 'or'">||</xsl:when>
				<xsl:otherwise>&amp;&amp;</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:text>(</xsl:text>

		<xsl:for-each select="./condition|conditions">
			<xsl:apply-templates select="." />
			<xsl:if test="position() &lt; last()">
				<div class="operator">
					<xsl:value-of select="$op"/>
				</div>
			</xsl:if>
		</xsl:for-each>

		<xsl:text>)</xsl:text>
	</xsl:template>

	<xsl:template match="condition">
		<xsl:variable name="op">
			<xsl:value-of select="./@op"/>
		</xsl:variable>
		<xsl:variable name="notOp">
			<xsl:value-of select="boolean(./@not = 'true')"/>
		</xsl:variable>

		<div class="operand">
			<xsl:apply-templates select="./input"/>
		</div>

		<div class="operator">
			<xsl:call-template name="translateOperator">
				<xsl:with-param name="op" select="$op"/>
				<xsl:with-param name="notOp" select="$notOp"/>
			</xsl:call-template>
		</div>

		<div class="operand">
			<xsl:apply-templates select="./control"/>
		</div>
	</xsl:template>

	<xsl:template match="input|control|value">
		<xsl:choose>
			<xsl:when test="./@source = 'input'"><div class="variable">input</div>[<div class="text">'</div></xsl:when>
			<xsl:when test="./@source = 'output'"><div class="variable">output</div>[<div class="text">'</div></xsl:when>
			<xsl:otherwise><div class="text">"</div></xsl:otherwise>
		</xsl:choose>
		<div class="text">
			<xsl:value-of select="."/>
		</div>
		<xsl:choose>
			<xsl:when test="./@source = 'input' or ./@source = 'output'">']</xsl:when>
			<xsl:otherwise><div class="text">"</div></xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="translateOperator">
		<xsl:param name="op"/>
		<xsl:param name="notOp" select="false()"/>
		<xsl:choose>
			<xsl:when test="$op = 'eq' and $notOp = 'true'">!=</xsl:when>
			<xsl:when test="$op = 'eq'">==</xsl:when>
			<xsl:when test="$op = 'lt' and $notOp = 'true'">&gt;=</xsl:when>
			<xsl:when test="$op = 'lt'">&lt;</xsl:when>
			<xsl:when test="$op = 'gt' and $notOp = 'true'">&lt;=</xsl:when>
			<xsl:when test="$op = 'gt'">&gt;</xsl:when>
			<xsl:when test="$op = 'ct' and $notOp = 'true'">!contains</xsl:when>
			<xsl:when test="$op = 'ct'">contains</xsl:when>
			<xsl:when test="$op = 'sw' and $notOp = 'true'">!starts_with</xsl:when>
			<xsl:when test="$op = 'sw'">starts_with</xsl:when>
			<xsl:when test="$op = 'ew' and $notOp = 'true'">!ends_with</xsl:when>
			<xsl:when test="$op = 'ew'">ends_with</xsl:when>
			<xsl:otherwise>
				<xsl:if test="$notOp">!</xsl:if>
				<xsl:value-of select="$op"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
